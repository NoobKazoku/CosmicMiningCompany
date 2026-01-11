using CosmicMiningCompany.scripts.command.game;
using CosmicMiningCompany.scripts.command.menu;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using CosmicMiningCompany.scripts.data.interfaces;
using GFramework.Core.command;
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
			_log.Debug("游戏设置");
			this.SendCommand(new OpenOptionsMenuCommand(new OpenOptionsMenuCommandInput()
			{
				Node = this
			}));
		};

		GetNode<Button>("%Credits").Pressed += () =>
		{
			GD.Print("制作人员名单");
			_log.Debug("制作人员名单");
		};

		GetNode<Button>("%Exit").Pressed += () =>
		{
			_log.Debug("退出游戏");
			this.SendCommand(new QuitGameCommand(new QuitGameCommandInput { Node = this }));
		};
	}
}
