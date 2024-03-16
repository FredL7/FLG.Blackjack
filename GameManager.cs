using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Godot;

using Blackjack;
using FLG.Cs.Framework;
using FLG.Cs.IDatamodel;
using FLG.Cs.Math;
using FLG.Cs.ServiceLocator;


public partial class GameManager : Node {
    private const string LOGS_RELATIVE_PATH = "./_logs";
    private const string LAYOUTS_RELATIVE_PATH = "./FLG.Framework/ProjectDefs.UI/Layouts";
    private const string PAGES_RELATIVE_PATH = "./FLG.Framework/ProjectDefs.UI/Pages";

    public override void _Ready()
    {
        InitializeFramework();
        var uiNode = GetNode("UI/Layouts");
        uiNode.Call("SetupUI");
    }

    private void InitializeFramework()
    {
        Preferences prefs = new();
        FrameworkManager.Instance.InitializeFramework(prefs);

        PreferencesLogs prefsLogs = new()
        {
            logsDir = LOGS_RELATIVE_PATH,
        };
        FrameworkManager.Instance.InitializeLogs(prefsLogs);

        PreferencesUI prefsUI = new()
        {
            layoutsDir = ProjectSettings.GlobalizePath("res://" + LAYOUTS_RELATIVE_PATH),
            pagesDir = ProjectSettings.GlobalizePath("res://" + PAGES_RELATIVE_PATH),
            windowSize = new Size(800, 600)
        };
        FrameworkManager.Instance.InitializeUI(prefsUI);

        IBlackJackManager blackjackManager = new BlackjackManager();
        FrameworkManager.Instance.Initialize(blackjackManager);
    }
}
