using CosmicMiningCompany.scripts.data;
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

	/// <summary>
	/// 从存档加载技能等级数据并更新PlayerManager变量
	/// </summary>
	/// <param name="playerManager">PlayerManager实例</param>
	private void LoadSkillLevelsFromSave(PlayerManager playerManager)
	{
		// 所有技能名称列表
		string[] skillNames = new string[]
		{
			"Acceleration", "Speed", "BrakeForce", "RotationSpeed",
			"MaxHeat", "ColdDownRateNormal", "ColdDownRateOverHeat", "ColdUpRateNormal",
			"MaxFuel", "FuelConsumptionRate", "OreGet", "PickupRange",
			"WeaponCount", "Damage", "FireRate"
		};

		// 技能数据读取工具
		var levelUpDataUtility = this.GetUtility<LevelUpDataReadUtility>();
		if (levelUpDataUtility == null)
		{
			_log.Error("无法获取LevelUpDataReadUtility工具");
			return;
		}

		foreach (var skillName in skillNames)
		{
			// 从存档获取技能等级
			int skillLevel = _saveStorageUtility.GetSkillLevel(skillName);
			
			// 如果玩家没有升过等级（存档返回0），则默认使用等级1
			if (skillLevel == 0)
			{
				skillLevel = 1;
			}

			// 获取该等级的技能数据
			var skillLevelData = levelUpDataUtility.GetSkillLevelData(skillName, skillLevel);
			if (skillLevelData != null)
			{
				// 根据技能名称更新PlayerManager中的对应变量
				UpdatePlayerManagerVariable(playerManager, skillName, (float)skillLevelData.Value);
				_log.Debug($"从存档加载技能 {skillName} 等级 {skillLevel}: 数值={skillLevelData.Value}");
			}
			else
			{
				_log.Debug($"未找到技能 {skillName} 等级 {skillLevel} 的数据");
			}
		}
	}

	/// <summary>
	/// 根据技能名称更新PlayerManager中的对应变量
	/// </summary>
	/// <param name="playerManager">PlayerManager实例</param>
	/// <param name="skillName">技能名称</param>
	/// <param name="value">技能数值</param>
	private void UpdatePlayerManagerVariable(PlayerManager playerManager, string skillName, float value)
	{
		switch (skillName)
		{
			case "Acceleration":
				playerManager.Acceleration = value;
				break;
			case "Speed":
				playerManager.Speed = value;
				break;
			case "BrakeForce":
				playerManager.BrakeForce = value;
				break;
			case "RotationSpeed":
				playerManager.RotationSpeed = value;
				break;
			case "MaxHeat":
				playerManager.MaxHeat = (int)value;
				break;
			case "ColdDownRateNormal":
				playerManager.ColdDownRateNormal = (int)value;
				break;
			case "ColdDownRateOverHeat":
				playerManager.ColdDownRateOverHeat = (int)value;
				break;
			case "ColdUpRateNormal":
				playerManager.ColdUpRateNormal = value;
				break;
			case "MaxFuel":
				playerManager.MaxFuel = value;
				break;
			case "FuelConsumptionRate":
				playerManager.FuelConsumptionRate = value;
				break;
			case "OreGet":
				playerManager.OreGet = (int)value;
				break;
			case "PickupRange":
				playerManager.PickupRange = (int)value;
				break;
			case "WeaponCount":
				playerManager.WeaponCount = (int)value;
				break;
			case "Damage":
				playerManager.Damage = (int)value;
				break;
			case "FireRate":
				playerManager.FireRate = value;
				break;
			default:
				_log.Error($"未知的技能名称: {skillName}");
				break;
		}
	}

}
