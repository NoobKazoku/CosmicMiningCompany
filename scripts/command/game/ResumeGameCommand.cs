using CosmicMiningCompany.scripts.game;
using GFramework.Core.Abstractions.command;
using GFramework.Core.command;
using GFramework.Core.extensions;
using Godot;

namespace CosmicMiningCompany.scripts.command.game;

/// <summary>
/// 恢复游戏命令类，用于取消游戏暂停状态
/// </summary>
/// <param name="input">恢复游戏命令输入参数</param>
public sealed class ResumeGameCommand(ResumeGameCommandInput input) : AbstractCommand<ResumeGameCommandInput>(input)
{
    /// <summary>
    /// 执行恢复游戏命令的具体逻辑
    /// </summary>
    /// <param name="input">恢复游戏命令输入参数，包含执行操作所需的节点信息</param>
    protected override void OnExecute(ResumeGameCommandInput input)
    {
        input.Node.GetTree().Paused = false;
        var gameStateModel = this.GetModel<IGameStateModel>()!;
        gameStateModel.SetGamePaused(false);
    }
}

/// <summary>
/// 恢复游戏命令输入类，封装执行恢复游戏操作所需的数据
/// </summary>
public sealed class ResumeGameCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置用于执行退出操作的节点对象
    /// </summary>
    public required Node Node { get; init; }
}
