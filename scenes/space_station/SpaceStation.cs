using CosmicMiningCompany.scripts.data.interfaces;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;


[ContextAware]
[Log]
public partial class SpaceStation :Control,IController
{
	private ISaveStorageUtility _saveStorageUtility = null!;
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		_saveStorageUtility = this.GetUtility<ISaveStorageUtility>()!;

		DataLoad();

		GetNode<Button>("%Depart").Pressed += () =>
		{
			GD.Print("离港");
			_log.Debug("离港");
			GetTree().ChangeSceneToFile("res://scenes/space/space.tscn");
		};
	}

	/// <summary>
	/// 初始化存档数据
	/// </summary>
	private void DataLoad()
	{
		// 加载存档
		_saveStorageUtility.Load();

		// 获取PlayerManager中的任务矿石数量
		var playerManager = PlayerManager.Instance;
		if (playerManager != null)
		{
			// 添加散矿到存档
			if (playerManager.OreCount > 0)
			{
				_saveStorageUtility.AddItem("Ore", playerManager.OreCount);
				_log.Debug($"添加散矿: {playerManager.OreCount}");
			}

			// 添加宝石到存档
			if (playerManager.GemCount > 0)
			{
				_saveStorageUtility.AddItem("Gem", playerManager.GemCount);
				_log.Debug($"添加宝石: {playerManager.GemCount}");
			}

			// 保存存档
			_saveStorageUtility.Save();

			// 更新PlayerManager中的总矿石量
			playerManager.UpdateTotalOreFromSave();

			// 重置任务矿石数量
			playerManager.OreCount = 0;
			playerManager.GemCount = 0;
		}
	}

}
