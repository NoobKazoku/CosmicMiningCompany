using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class MainMenu :Control,IController
{
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	


	public override void _Ready()
	{
		GetNode<Button>("%NewGame").Pressed += () =>
		{
			GD.Print("新游戏");
			_log.Debug("新游戏");
			GetTree().ChangeSceneToFile("res://scenes/space_station/space_station.tscn");
		};


		GetNode<Button>("%Continue").Pressed += () =>
		{
			GD.Print("继续游戏");
			_log.Debug("继续游戏");
		};


		GetNode<Button>("%Option").Pressed += () =>
		{
			GD.Print("游戏设置");
			_log.Debug("游戏设置");
		};


		GetNode<Button>("%Credits").Pressed += () =>
		{
			GD.Print("制作人员名单");
			_log.Debug("制作人员名单");
		};


		GetNode<Button>("%Exit").Pressed += () =>
		{
			GD.Print("退出游戏");
			_log.Debug("退出游戏");
			GetTree().Quit();
		};
	}
}


