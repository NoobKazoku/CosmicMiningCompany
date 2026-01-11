using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class GameOver :Control,IController
{
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		GetNode<Button>("%Button").Pressed += () =>
		{
			GD.Print("返回空间站");
			_log.Debug("返回空间站");

			QueueFree();
			GetTree().Paused = false;
			
			GetTree().ChangeSceneToFile("res://scenes/space_station/space_station.tscn");
		};
	}


}


