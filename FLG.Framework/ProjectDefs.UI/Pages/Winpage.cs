using FLG.Cs.IDatamodel;
using FLG.Cs.ServiceLocator;
using Blackjack;

public class Winpage : IPage {
    private const string PAGE_ID = "Winpage";

    public string PageId { get => PAGE_ID; }
    public string LayoutId { get; set; } = "";

    IText _text;

    public void Setup()
    {
        var factory = Locator.Instance.Get<IUIFactory>();
        var ui = Locator.Instance.Get<IUIManager>();
        var layout = ui.GetLayout(LayoutId);

        _text = (IText)factory.Text("win-text", "", new() { Width = 300});
        layout.GetTarget("win-section").AddChild(_text);

        var btn = factory.Button("replay-btn", "Play again", OnPlayAgainBtnClicked, new() { Height = 30, Width = 200 });
        layout.GetTarget("replay-section").AddChild(btn);
    }

    public void OnPlayAgainBtnClicked()
    {
        var blackjack = Locator.Instance.Get<IBlackJackManager>();
        blackjack.Reset();

        var ui = Locator.Instance.Get<IUIManager>();
        ui.SetCurrentPage("Gamepage");
    }

    public void OnOpen()
    {
        var blackjack = Locator.Instance.Get<IBlackJackManager>();

        var dealerHand = blackjack.DealerHand;
        var dealerTotal = blackjack.HandTotal(dealerHand);
        var playerHand = blackjack.PlayerHand;
        var playerTotal = blackjack.HandTotal(playerHand);

        if (playerTotal > 21)
        {
            _text.Content = $"Player lost - Busted ({playerTotal} vs {dealerTotal})";
        }
        else if (playerTotal == 21)
        {
            _text.Content = $"Player win - Blackjack ({playerTotal} vs {dealerTotal})";
        }
        else if (playerTotal == dealerTotal)
        {
            _text.Content = $"No winner - Tie ({playerTotal} vs {dealerTotal})";
        }
        else if (dealerTotal > 21)
        {
            _text.Content = $"Player win - Dealer busted ({playerTotal} vs {dealerTotal})";
        }
        else if (playerTotal > dealerTotal)
        {
            _text.Content = $"Player win - better cards ({playerTotal} vs {dealerTotal})";
        }
        else if (playerTotal < dealerTotal)
        {
            _text.Content = $"Player loss - better cards ({playerTotal} vs {dealerTotal})";
        }
    }

    public void OnClose() { }
}
