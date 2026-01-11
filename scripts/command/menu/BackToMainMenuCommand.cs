using GFramework.Core.Abstractions.command;
using GFramework.Core.command;
using Godot;

namespace CosmicMiningCompany.scripts.command.menu;

/// <summary>
/// 返回主菜单命令类，用于处理从当前场景返回到主菜单的逻辑
/// </summary>
/// <param name="input">命令输入参数，包含执行命令所需的节点信息</param>
public sealed class BackToMainMenuCommand(BackToMainMenuCommandInput input)
    : AbstractCommand<BackToMainMenuCommandInput>(input)
{
    /// <summary>
    /// 执行返回主菜单命令的具体逻辑
    /// </summary>
    /// <param name="input">命令输入参数，包含执行命令所需的节点信息</param>
    protected override void OnExecute(BackToMainMenuCommandInput input)
    {
        // 获取场景树并恢复暂停状态，然后切换到主菜单场景
        var tree = input.Node.GetTree();
        tree.Paused = false;
        tree.ChangeSceneToFile("res://scenes/main_menu/main_menu.tscn");
    }
}

/// <summary>
/// 返回主菜单命令的输入参数类
/// </summary>
public sealed class BackToMainMenuCommandInput : ICommandInput
{
    /// <summary>
    /// 执行命令所需的节点引用，用于获取场景树
    /// </summary>
    public required Node Node { get; init; }
}
