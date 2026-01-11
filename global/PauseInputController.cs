using CosmicMiningCompany.scripts.command.game;
using CosmicMiningCompany.scripts.core;
using CosmicMiningCompany.scripts.events.menu;
using CosmicMiningCompany.scripts.game;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.global;

/// <summary>
/// 暂停输入控制器，处理游戏暂停和恢复的输入逻辑
/// </summary>
[ContextAware]
[Log]
public partial class PauseInputController : GameInputController
{
    private IGameStateModel _gameStateModel = null!;
    public override void _Ready()
    {
        _gameStateModel = this.GetModel<IGameStateModel>()!;
    }
    /// <summary>
    /// 处理输入事件，检测暂停/恢复游戏的按键操作
    /// </summary>
    /// <param name="event">输入事件对象</param>
    protected override void HandleInput(InputEvent @event)
    {
        // 检查是否按下了取消键（通常是ESC键）
        if (!@event.IsActionPressed("ui_cancel"))
        {
            return;
        }

        if (!_gameStateModel.IsGaming())
        {
            return;
        }
        // 根据当前游戏暂停状态决定执行暂停或恢复命令
        if (_gameStateModel.IsGamePaused())
        {
            // 当前是暂停 → 恢复游戏 → 关闭暂停菜单
            this.SendEvent<ClosePauseMenuEvent>();
            this.SendCommand(new ResumeGameCommand(new ResumeGameCommandInput{Node = this}));
        }
        else
        {
            // 当前是运行 → 暂停游戏 → 打开暂停菜单
            this.SendCommand(new PauseGameCommand(new PauseGameCommandInput{Node = this}));
            this.SendEvent<OpenPauseMenuEvent>();
        }

    }
}
