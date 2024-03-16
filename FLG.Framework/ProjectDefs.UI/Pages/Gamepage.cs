using System.Collections.Generic;

using Godot;

using FLG.Cs.IDatamodel;
using FLG.Cs.ServiceLocator;
using Blackjack;


public class Gamepage : IPage {
    private const string PAGE_ID = "Gamepage";

    public string PageId { get => PAGE_ID; }
    public string LayoutId { get; set; } = "";

    IText _dealerHand, _playerHand;
    IText _dealerLabel, _playerLabel;

    public void Setup()
    {
        var factory = Locator.Instance.Get<IUIFactory>();
        var ui = Locator.Instance.Get<IUIManager>();
        var layout = ui.GetLayout(LayoutId);

        _dealerLabel = (IText)factory.Text("dealer-label", "", new() { Width = 128, Height = 64, Margin = new(20, 0, 0, 0) });
        _dealerHand = (IText)factory.Text("dealer-hand", "", new() { Height = 64 });
        layout.GetTarget("dealer-section").AddChild(_dealerLabel);
        layout.GetTarget("dealer-section").AddChild(_dealerHand);

        _playerLabel = (IText)factory.Text("player-label", "", new() { Width = 128, Height = 64, Margin = new(20, 0, 0, 0) });
        _playerHand = (IText)factory.Text("player-hand", "", new() { Height = 64 });
        layout.GetTarget("player-section").AddChild(_playerLabel);
        layout.GetTarget("player-section").AddChild(_playerHand);

        var dealBtn = factory.Button("deal-btn", "Deal", OnDealBtnClicked, new() { Width = 128 });
        var checkBtn = factory.Button("check-btn", "Stand", OnCheckBtnClicked, new() { Width = 128, Margin = new(0, 20, 0, 0) });
        var controlTarget = layout.GetTarget("game-controls");
        controlTarget.AddChild(dealBtn);
        controlTarget.AddChild(checkBtn);
    }

    private void CheckForWin()
    {
        var blackjack = Locator.Instance.Get<IBlackJackManager>();
        var total = blackjack.HandTotal(blackjack.PlayerHand);
        if (total >= 21)
        {
            var ui = Locator.Instance.Get<IUIManager>();
            ui.SetCurrentPage("Winpage");
        }
    }

    public void OnDealBtnClicked()
    {
        var blackjack = Locator.Instance.Get<IBlackJackManager>();
        blackjack.DrawPlayer();
        DrawPlayerHand(blackjack);

        CheckForWin();
    }

    public void OnCheckBtnClicked()
    {
        var blackjack = Locator.Instance.Get<IBlackJackManager>();
        blackjack.DealerAIPlay();

        var ui = Locator.Instance.Get<IUIManager>();
        ui.SetCurrentPage("Winpage");
    }

    public void OnOpen()
    {
        var blackjack = Locator.Instance.Get<IBlackJackManager>();
        DrawDealerHand(blackjack);
        DrawPlayerHand(blackjack);

        CheckForWin();
    }

    private void DrawDealerHand(IBlackJackManager blackjack)
    {
        var dealerHand = blackjack.DealerHand;
        _dealerLabel.Content = $"Dealer hand";
        _dealerHand.Content = DrawHand(dealerHand, true);
    }

    private void DrawPlayerHand(IBlackJackManager blackjack)
    {
        var playerHand = blackjack.PlayerHand;
        var total = blackjack.HandTotal(playerHand);
        _playerLabel.Content = $"Player hand ({total})";
        _playerHand.Content = DrawHand(playerHand, false);
    }

    public void OnClose() { }

    private string DrawHand(List<ICard> hand, bool hideFirst)
    {
        string text = "";
        for (int i = 0; i < hand.Count; ++i)
        {
            if (i == 0 && hideFirst)
            {
                text += HiddenCardBBCode();
                continue;
            }
            else
            {
                text += CardBBCode(hand[i]);
            }
        }
        return text;
    }

    private string HiddenCardBBCode()
    {
        return $"[img width=64 height=64 region={5 * 64},{6 * 64},64,64]spritesheet-cards-alpha.png[/img]";
    }

    private string CardBBCode(ICard card)
    {
        var index = (card.Number - 1) * 4;
        switch (card.Color)
        {
            case "DIAMONDS": index += 0; break;
            case "HEARTS": index += 1; break;
            case "SPADES": index += 2; break;
            case "CLUBS": index += 3; break;
        }

        int x = (index % 8);
        int y = (index / 8);

        return $"[img width=64 height=64 region={x * 64},{y * 64},64,64]spritesheet-cards-alpha.png[/img]";
    }
}
