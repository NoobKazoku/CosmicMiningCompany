using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.credits;

[ContextAware]
[Log]
public partial class Credits : Control, IController
{
    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        // 设置返回主菜单按钮的点击事件
        GetNode<Button>("%Back").Pressed += () =>
        {
            GD.Print("返回主菜单");
            _log.Debug("返回主菜单");
            GetTree().ChangeSceneToFile("res://scenes/main_menu/main_menu.tscn");
        };
    }
}