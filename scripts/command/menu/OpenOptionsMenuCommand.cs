using CosmicMiningCompany.scripts.events.menu;
using GFramework.Core.Abstractions.command;
using GFramework.Core.command;
using GFramework.Core.extensions;
using Godot;

namespace CosmicMiningCompany.scripts.command.menu;

/// <summary>
/// 打开选项菜单命令类
/// 负责处理打开选项菜单的命令执行逻辑
/// </summary>
/// <param name="input">空输入参数对象</param>
public class OpenOptionsMenuCommand(OpenOptionsMenuCommandInput input) : AbstractCommand<OpenOptionsMenuCommandInput>(input)
{
    /// <summary>
    /// 执行命令的核心方法
    /// 发送打开选项菜单事件以触发相应的UI显示逻辑
    /// </summary>
    /// <param name="input">空输入参数对象</param>
    protected override void OnExecute(OpenOptionsMenuCommandInput input)
    {
        // 发送打开选项菜单事件
        this.SendEvent(new OpenOptionsMenuEvent()
        {
            Node = input.Node
        });
    }
}
public sealed class OpenOptionsMenuCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置用于执行命令的节点对象
    /// </summary>
    public required Node Node { get; init; }
}
