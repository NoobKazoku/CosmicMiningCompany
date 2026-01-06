using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class SpaceStation :Node2D,IController
{
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		GetNode<Button>("%Depart").Pressed += () =>
		{
			GD.Print("离港");
			_log.Debug("离港");
			GetTree().ChangeSceneToFile("res://scene/space/space.tscn");
		};
	}
}


