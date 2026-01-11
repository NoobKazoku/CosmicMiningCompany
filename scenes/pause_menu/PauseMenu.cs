using CosmicMiningCompany.scripts.command.game;
using CosmicMiningCompany.scripts.command.menu;
using CosmicMiningCompany.scripts.events.menu;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.pause_menu;

/// <summary>
/// 暂停菜单控制器，负责处理游戏暂停时的界面交互逻辑
/// 实现了IController接口，支持上下文感知和日志记录功能
/// </summary>
[ContextAware]
[Log]
public partial class PauseMenu : Control, IController
{
    /// <summary>
    /// 获取恢复游戏按钮节点
    /// </summary>
    private Button ResumeButton => GetNode<Button>("%ResumeButton");
    
    /// <summary>
    /// 获取选项按钮节点
    /// </summary>
    private Button OptionsButton => GetNode<Button>("%OptionsButton");
    
    /// <summary>
    /// 获取主菜单按钮节点
    /// </summary>
    private Button MainMenuButton => GetNode<Button>("%MainMenuButton");
    
    /// <summary>
    /// 获取退出游戏按钮节点
    /// </summary>
    private Button QuitButton => GetNode<Button>("%QuitButton");

    /// <summary>
    /// 节点就绪时调用的方法，用于初始化UI和设置事件处理器
    /// </summary>
    public override void _Ready()
    {
        InitializeUi();
        SetupEventHandlers();
    }

    /// <summary>
    /// 设置按钮点击事件处理器
    /// 为各个按钮绑定相应的命令发送逻辑
    /// </summary>
    private void SetupEventHandlers()
    {
        // 绑定恢复游戏按钮点击事件
        ResumeButton.Pressed += () =>
        {
            this.SendEvent<ClosePauseMenuEvent>();
            this.SendCommand(new ResumeGameCommand(new ResumeGameCommandInput { Node = this }));
        };

        // 绑定选项按钮点击事件
        OptionsButton.Pressed += () => 
        {
            this.SendCommand(new OpenOptionsMenuCommand(new OpenOptionsMenuCommandInput()
            {
                Node = this
            })); 
        };

        // 绑定返回主菜单按钮点击事件
        MainMenuButton.Pressed += () =>
        {
            this.SendEvent<ClosePauseMenuEvent>();
            this.SendCommand(new BackToMainMenuCommand(new BackToMainMenuCommandInput { Node = this }));
        };
        
        // 绑定退出游戏按钮点击事件
        QuitButton.Pressed += () =>
        {
            this.SendEvent<ClosePauseMenuEvent>();
            this.SendCommand(new QuitGameCommand(new QuitGameCommandInput { Node = this }));
        };
    }

    /// <summary>
    /// 初始化用户界面组件
    /// 当前为空实现，可在此方法中进行UI组件的初始化配置
    /// </summary>
    private void InitializeUi()
    {
    }
}
