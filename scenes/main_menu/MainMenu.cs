using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using CosmicMiningCompany.scripts.data.interfaces;
using Godot;
using GFramework.Core.extensions;

namespace CosmicMiningCompany.scenes.main_menu;

[ContextAware]
[Log]
public partial class MainMenu : Control, IController
{
	private ISaveStorageUtility _saveStorageUtility = null!;
	
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		// 获取存档工具依赖
		_saveStorageUtility = this.GetUtility<ISaveStorageUtility>()!;

		GetNode<Button>("%NewGame").Pressed += () =>
		{
			GD.Print("新游戏");
			_log.Debug("新游戏");
			
			// 创建新存档
			_saveStorageUtility.NewGame();
			
			GetTree().ChangeSceneToFile("res://scenes/space_station/space_station.tscn");
		};

		GetNode<Button>("%Continue").Pressed += () =>
		{
			GD.Print("继续游戏");
			_log.Debug("继续游戏");
			
			// 加载现有存档
			_saveStorageUtility.Load();
			
			GetTree().ChangeSceneToFile("res://scenes/space_station/space_station.tscn");
		};

		GetNode<Button>("%Option").Pressed += () =>
		{
			GD.Print("游戏设置");
			_log.Debug("游戏设置");
			// 实例化设置场景
			var optionScene = GD.Load<PackedScene>("res://scenes/Options/Options.tscn").Instantiate() as Control;
			// 将其添加到当前场景中
			AddChild(optionScene);
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
