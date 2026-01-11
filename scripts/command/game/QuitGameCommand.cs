using GFramework.Core.Abstractions.command;
using GFramework.Core.command;
using Godot;

namespace CosmicMiningCompany.scripts.command.game;

/// <summary>
/// 退出游戏命令类，用于处理游戏退出逻辑
/// </summary>
/// <param name="input">退出游戏命令输入参数</param>
public sealed class QuitGameCommand(QuitGameCommandInput input) : AbstractCommand<QuitGameCommandInput>(input)
{
    /// <summary>
    /// 执行退出游戏命令的具体逻辑
    /// </summary>
    /// <param name="input">退出游戏命令输入参数，包含执行退出操作所需的节点信息</param>
    protected override void OnExecute(QuitGameCommandInput input)
    {
        input.Node.GetTree().Quit();
    }
}

/// <summary>
/// 退出游戏命令输入类，封装执行退出游戏命令所需的数据
/// </summary>
public sealed class QuitGameCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置用于执行退出操作的节点对象
    /// </summary>
    public required Node Node { get; init; }
}
