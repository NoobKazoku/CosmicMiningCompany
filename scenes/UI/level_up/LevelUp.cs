using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using CosmicMiningCompany.scripts.data;
using CosmicMiningCompany.scripts.data.interfaces;
using GFramework.Core.extensions;
using System.IO;

[ContextAware]
[Log]
public partial class LevelUp : Button, IController
{
	[Export] public string VariableName = ""; //升级的变量名称
	[Export] public int VariableValue = 0; //升级后的变量值
	[Export] public int OreCost = 0; //升级的散矿花费
	[Export] public int GemCost = 0; //升级的宝石花费

	// 技能数据读取工具
	private LevelUpDataReadUtility _levelUpDataUtility = null!;
	// 存档数据工具
	private ISaveStorageUtility _saveStorageUtility = null!;
	// 当前选中的技能数据
	private SkillData? _currentSkillData = null;
	// 当前选中的等级数据
	private SkillLevel? _currentSkillLevel = null;
	
	// 静态缓存：参考陨石实现方法的备选方案
	private static SkillRoot? _cachedSkillRoot = null;
	private static bool _hasTriedLoadStatic = false;

	public override void _Ready()
	{
		// 初始化技能数据读取工具
		_levelUpDataUtility = this.GetUtility<LevelUpDataReadUtility>()!;
		// 初始化存档数据工具
		_saveStorageUtility = this.GetUtility<ISaveStorageUtility>()!;
		
		// 调试日志
		GD.Print($"LevelUp._Ready(): _levelUpDataUtility = {_levelUpDataUtility}");
		GD.Print($"LevelUp._Ready(): _saveStorageUtility = {_saveStorageUtility}");
		
		this.Pressed += this.OnPressed;
		OnButtonsPressed();
	}

	/// <summary>
	/// 升级按钮点击事件处理
	/// </summary>
	public void OnPressed()
	{
		// 检查是否有选中的技能
		if (_currentSkillData == null || _currentSkillLevel == null)
		{
			GD.Print("请先选择一个技能");
			return;
		}
		
		// 获取当前技能等级
		int currentLevel = _saveStorageUtility.GetSkillLevel(_currentSkillData.Name);
		if (currentLevel == 0)
		{
			currentLevel = 1;
		}
		
		// 检查是否已满级
		if (currentLevel >= _currentSkillData.MaxLevel)
		{
			GD.Print($"{_currentSkillData.DisplayName} 已满级，无法继续升级");
			return;
		}
		
		// 检查矿石是否足够
		if (PlayerManager.Instance == null)
		{
			GD.PrintErr("PlayerManager未初始化");
			return;
		}
		
		// 检查散矿是否足够
		if (PlayerManager.Instance.TotalOreCount < OreCost)
		{
			GD.Print($"矿石不足！需要{OreCost}散矿，当前只有{PlayerManager.Instance.TotalOreCount}散矿");
			return;
		}
		
		// 检查宝石是否足够
		if (PlayerManager.Instance.TotalGemCount < GemCost)
		{
			GD.Print($"宝石不足！需要{GemCost}宝石，当前只有{PlayerManager.Instance.TotalGemCount}宝石");
			return;
		}
		
		// 消耗矿石
		bool oreConsumed = _saveStorageUtility.ConsumeItem("Ore", OreCost);
		bool gemConsumed = _saveStorageUtility.ConsumeItem("Gem", GemCost);
		
		if (!oreConsumed || !gemConsumed)
		{
			GD.PrintErr("消耗矿石失败");
			return;
		}
		
		// 更新PlayerManager中的矿石数量
		PlayerManager.Instance.TotalOreCount -= OreCost;
		PlayerManager.Instance.TotalGemCount -= GemCost;
		
		// 提升技能等级
		int nextLevel = currentLevel + 1;
		_saveStorageUtility.SetSkillLevel(_currentSkillData.Name, nextLevel);
		
		// 保存存档
		_saveStorageUtility.Save();
		
		GD.Print($"升级成功！{_currentSkillData.DisplayName} 从等级{currentLevel}提升到等级{nextLevel}");
		GD.Print($"消耗了{OreCost}散矿和{GemCost}宝石");
		
		// 更新PlayerManager中的对应变量
		UpdatePlayerManagerVariable(_currentSkillData.Name, (float)_currentSkillLevel.Value);
		
		// 重新选择技能以更新显示
		SelectSkill(_currentSkillData.Name);
	}
	
	/// <summary>
	/// 根据技能名称更新PlayerManager中的对应变量
	/// </summary>
	/// <param name="skillName">技能名称</param>
	/// <param name="value">技能数值</param>
	private void UpdatePlayerManagerVariable(string skillName, float value)
	{
		if (PlayerManager.Instance == null)
		{
			GD.PrintErr("PlayerManager未初始化");
			return;
		}
		
		switch (skillName)
		{
			case "Acceleration":
				PlayerManager.Instance.Acceleration = value;
				GD.Print($"更新PlayerManager.Acceleration = {value}");
				break;
			case "Speed":
				PlayerManager.Instance.Speed = value;
				GD.Print($"更新PlayerManager.Speed = {value}");
				break;
			case "BrakeForce":
				PlayerManager.Instance.BrakeForce = value;
				GD.Print($"更新PlayerManager.BrakeForce = {value}");
				break;
			case "RotationSpeed":
				PlayerManager.Instance.RotationSpeed = value;
				GD.Print($"更新PlayerManager.RotationSpeed = {value}");
				break;
			case "MaxHeat":
				PlayerManager.Instance.MaxHeat = (int)value;
				GD.Print($"更新PlayerManager.MaxHeat = {value}");
				break;
			case "ColdDownRateNormal":
				PlayerManager.Instance.ColdDownRateNormal = (int)value;
				GD.Print($"更新PlayerManager.ColdDownRateNormal = {value}");
				break;
			case "ColdDownRateOverHeat":
				PlayerManager.Instance.ColdDownRateOverHeat = (int)value;
				GD.Print($"更新PlayerManager.ColdDownRateOverHeat = {value}");
				break;
			case "ColdUpRateNormal":
				PlayerManager.Instance.ColdUpRateNormal = value;
				GD.Print($"更新PlayerManager.ColdUpRateNormal = {value}");
				break;
			case "MaxFuel":
				PlayerManager.Instance.MaxFuel = value;
				GD.Print($"更新PlayerManager.MaxFuel = {value}");
				break;
			case "FuelConsumptionRate":
				PlayerManager.Instance.FuelConsumptionRate = value;
				GD.Print($"更新PlayerManager.FuelConsumptionRate = {value}");
				break;
			case "OreGet":
				PlayerManager.Instance.OreGet = (int)value;
				GD.Print($"更新PlayerManager.OreGet = {value}");
				break;
			case "PickupRange":
				PlayerManager.Instance.PickupRange = (int)value;
				GD.Print($"更新PlayerManager.PickupRange = {value}");
				break;
			case "WeaponCount":
				PlayerManager.Instance.WeaponCount = (int)value;
				GD.Print($"更新PlayerManager.WeaponCount = {value}");
				break;
			case "Damage":
				PlayerManager.Instance.Damage = (int)value;
				GD.Print($"更新PlayerManager.Damage = {value}");
				break;
			case "FireRate":
				PlayerManager.Instance.FireRate = value;
				GD.Print($"更新PlayerManager.FireRate = {value}");
				break;
			default:
				GD.PrintErr($"未知的技能名称: {skillName}");
				break;
		}
	}
	
	public void OnButtonsPressed()
	{
		GetNode<Button>("%Acceleration").Pressed += () =>
		{
			SelectSkill("Acceleration");
		};

		GetNode<Button>("%Speed").Pressed += () =>
		{
			SelectSkill("Speed");
		};

		GetNode<Button>("%BrakeForce").Pressed += () =>
		{
			SelectSkill("BrakeForce");
		};

		GetNode<Button>("%RotationSpeed").Pressed += () =>
		{
			SelectSkill("RotationSpeed");
		};

		GetNode<Button>("%MaxHeat").Pressed += () =>
		{
			SelectSkill("MaxHeat");
		};

		GetNode<Button>("%ColdDownRateNormal").Pressed += () =>
		{
			SelectSkill("ColdDownRateNormal");
		};

		GetNode<Button>("%ColdDownRateOverHeat").Pressed += () =>
		{
			SelectSkill("ColdDownRateOverHeat");
		};

		GetNode<Button>("%ColdUpRateNormal").Pressed += () =>
		{
			SelectSkill("ColdUpRateNormal");
		};

		GetNode<Button>("%MaxFuel").Pressed += () =>
		{
			SelectSkill("MaxFuel");
		};

		GetNode<Button>("%FuelConsumptionRate").Pressed += () =>
		{
			SelectSkill("FuelConsumptionRate");
		};

		GetNode<Button>("%OreGet").Pressed += () =>
		{
			SelectSkill("OreGet");
		};

		GetNode<Button>("%PickupRange").Pressed += () =>
		{
			SelectSkill("PickupRange");
		};

		GetNode<Button>("%WeaponCount").Pressed += () =>
		{
			SelectSkill("WeaponCount");
		};

		GetNode<Button>("%Damage").Pressed += () =>
		{
			SelectSkill("Damage");
		};

		GetNode<Button>("%FireRate").Pressed += () =>
		{
			SelectSkill("FireRate");
		};
	}
	
	/// <summary>
	/// 选择技能
	/// </summary>
	/// <param name="skillName">技能名称</param>
	private void SelectSkill(string skillName)
	{
		// 方法1：使用依赖注入的工具类
		_currentSkillData = _levelUpDataUtility.GetSkillData(skillName);
		
		// 方法2：如果依赖注入失败，使用静态工具类作为备选方案（参考陨石实现方法）
		if (_currentSkillData == null)
		{
			GD.Print("依赖注入工具类返回null，尝试使用静态工具类...");
			_currentSkillData = GetSkillDataStatic(skillName);
		}
		
		if (_currentSkillData != null)
		{
			GD.Print($"选中{_currentSkillData.DisplayName}: {_currentSkillData.Description}");
			
			// 从存档中获取当前技能等级
			int currentLevel = _saveStorageUtility.GetSkillLevel(skillName);
			
			// 如果玩家没有升过等级（存档返回0），则默认使用等级1
			if (currentLevel == 0)
			{
				currentLevel = 1;
			}
			
			// 计算下一等级（如果当前等级小于最大等级，则下一等级为当前等级+1，否则为当前等级）
			int nextLevel = currentLevel;
			if (currentLevel < _currentSkillData.MaxLevel)
			{
				nextLevel = currentLevel + 1;
			}
			
			// 获取下一等级的技能数据
			_currentSkillLevel = _levelUpDataUtility.GetSkillLevelData(skillName, nextLevel);
			
			// 如果依赖注入失败，使用静态工具类
			if (_currentSkillLevel == null)
			{
				_currentSkillLevel = GetSkillLevelDataStatic(skillName, nextLevel);
			}
			
			if (_currentSkillLevel != null)
			{
				// 更新Export变量，供其他系统使用
				VariableName = skillName;
				VariableValue = (int)_currentSkillLevel.Value;
				OreCost = _currentSkillLevel.UpgradeCost.Ore;
				GemCost = _currentSkillLevel.UpgradeCost.Gem;
				
				if (currentLevel < _currentSkillData.MaxLevel)
				{
					GD.Print($"当前等级{currentLevel} -> 下一等级{nextLevel}: 数值={_currentSkillLevel.Value}, 消耗={OreCost}矿石, {GemCost}宝石");
				}
				else
				{
					GD.Print($"已满级（等级{currentLevel}）: 数值={_currentSkillLevel.Value}");
				}
			}
			else
			{
				GD.PrintErr($"未找到技能 {skillName} 的等级 {nextLevel} 数据");
			}
		}
		else
		{
			GD.PrintErr($"未找到技能: {skillName}");
		}
	}
	
	/// <summary>
	/// 使用静态工具类获取技能数据（参考陨石实现方法）
	/// </summary>
	private SkillData? GetSkillDataStatic(string skillName)
	{
		try
		{
			// 确保缓存已加载
			LoadSkillDataStatic();
			
			if (_cachedSkillRoot != null)
			{
				return LevelUpDataHelper.GetSkillData(_cachedSkillRoot, skillName);
			}
		}
		catch (System.Exception ex)
		{
			GD.PrintErr($"静态工具类读取技能数据失败: {ex.Message}");
		}
		
		return null;
	}
	
	/// <summary>
	/// 使用静态工具类获取技能等级数据（参考陨石实现方法）
	/// </summary>
	private SkillLevel? GetSkillLevelDataStatic(string skillName, int level)
	{
		try
		{
			// 确保缓存已加载
			LoadSkillDataStatic();
			
			if (_cachedSkillRoot != null)
			{
				return LevelUpDataHelper.GetSkillLevelData(_cachedSkillRoot, skillName, level);
			}
		}
		catch (System.Exception ex)
		{
			GD.PrintErr($"静态工具类读取技能等级数据失败: {ex.Message}");
		}
		
		return null;
	}
	
	/// <summary>
	/// 使用静态方法加载技能数据（参考陨石实现方法）
	/// </summary>
	private void LoadSkillDataStatic()
	{
		if (_hasTriedLoadStatic)
			return;
			
		try
		{
			_hasTriedLoadStatic = true;
			
			// LevelUp.json文件路径
			string filePath = ProjectSettings.GlobalizePath("res://assets/data/LevelUp.json");
			GD.Print($"尝试从静态路径加载技能数据: {filePath}");
			
			if (File.Exists(filePath))
			{
				string json = File.ReadAllText(filePath);
				_cachedSkillRoot = LevelUpDataHelper.LoadFromJson(json);
				GD.Print($"静态工具类加载成功，找到 {(_cachedSkillRoot?.Skills?.Count ?? 0)} 个技能");
			}
			else
			{
				GD.PrintErr($"LevelUp.json文件不存在: {filePath}");
			}
		}
		catch (System.Exception ex)
		{
			GD.PrintErr($"静态工具类加载技能数据失败: {ex.Message}");
		}
	}
}
