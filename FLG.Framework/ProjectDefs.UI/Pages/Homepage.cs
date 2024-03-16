using FLG.Cs.IDatamodel;
using FLG.Cs.ServiceLocator;

public class Homepage : IPage {
    private const string PAGE_ID = "Homepage";

    public string PageId { get => PAGE_ID; }
    public string LayoutId { get; set; } = "";

    public void Setup()
    {
        var factory = Locator.Instance.Get<IUIFactory>();
        var btnPlay = factory.Button("play-btn", "Play", OnPlayBtnClicked, new() { Height = 30, Width=200 });

        var ui = Locator.Instance.Get<IUIManager>();
        var layout = ui.GetLayout(LayoutId);
        var target = layout.GetTarget("controls-section");
        target.AddChild(btnPlay, PageId);
    }

    public void OnPlayBtnClicked()
    {
        var ui = Locator.Instance.Get<IUIManager>();
        ui.SetCurrentPage("Gamepage");
    }

    public void OnOpen() { }
    public void OnClose() { }
}
