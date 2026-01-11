using CosmicMiningCompany.scripts.command.game;
using CosmicMiningCompany.scripts.command.menu;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.pause_menu;

[ContextAware]
[Log]
public partial class PauseMenu : Control, IController
{
    private Button ResumeButton => GetNode<Button>("%ResumeButton");
    private Button OptionsButton => GetNode<Button>("%OptionsButton");
    private Button MainMenuButton => GetNode<Button>("%MainMenuButton");
    private Button QuitButton => GetNode<Button>("%QuitButton");
    private IPauseMenuSystem _pauseMenuSystem;

    public override void _Ready()
    {
        InitializeUi();
        SetupEventHandlers();
    }

    private void SetupEventHandlers()
    {
        ResumeButton.Pressed += () =>
        {
            this.SendCommand(new ResumeGameCommand(new ResumeGameCommandInput { Node = this }));
            _pauseMenuSystem.Close();
        };

        OptionsButton.Pressed += () => { this.SendCommand(new OpenOptionsMenuCommand(new OpenOptionsMenuCommandInput()
        {
            Node = this
        })); };

        MainMenuButton.Pressed += () =>
        {
            this.SendCommand(new BackToMainMenuCommand(new BackToMainMenuCommandInput { Node = this }));
            _pauseMenuSystem.Close();
        };
        QuitButton.Pressed += () =>
        {
            this.SendCommand(new QuitGameCommand(new QuitGameCommandInput { Node = this }));
            _pauseMenuSystem.Close();
        };
    }

    private void InitializeUi()
    {
        _pauseMenuSystem = this.GetSystem<IPauseMenuSystem>()!;
    }
}