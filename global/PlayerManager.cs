using System;
using Godot;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using GFramework.Core.extensions;
using CosmicMiningCompany.scripts.data.interfaces;

/// <summary>
/// 玩家管理器类，负责管理玩家的各种属性和状态
/// 采用单例模式，提供全局访问点
/// </summary>
[ContextAware]
[Log]
public partial class PlayerManager : Node
{
    /// <summary>
    /// 获取玩家管理器的单例实例
    /// </summary>
    public static PlayerManager Instance { get; private set; }
    
    /// <summary>
    /// 飞船最大燃料值
    /// </summary>
    public float MaxFuel = 10.0f;
    
    /// <summary>
    /// 燃料消耗速率，每秒消耗的燃料量
    /// </summary>
    public float FuelConsumptionRate = 0.333f;

    /// <summary>
    /// 武器数量
    /// </summary>
    public int WeaponCount = 2;
    
    /// <summary>
    /// 子弹伤害值
    /// </summary>
    public int Damage = 5;
    
    /// <summary>
    /// 射击间隔，单位秒
    /// </summary>
    public float FireRate = 0.2f;

    /// <summary>
    /// 飞船最大速度
    /// </summary>
    public float Speed = 300.0f;
    
    /// <summary>
    /// 飞船加速度
    /// </summary>
    public float Acceleration = 200.0f;
    
    /// <summary>
    /// 飞船制动力
    /// </summary>
    public float BrakeForce = 150.0f;
    
    /// <summary>
    /// 飞船转向角速度
    /// </summary>
    public float RotationSpeed = 5.0f;

    /// <summary>
    /// 每次采矿获得的矿物数量
    /// </summary>
    public int OreGet = 10;
    
    /// <summary>
    /// 战利品拾取范围大小倍率
    /// </summary>
    public int PickupRange = 1;

    /// <summary>
    /// 当前任务中获得的散矿数量
    /// </summary>
    public int OreCount = 0;
    
    /// <summary>
    /// 当前任务中获得的宝石数量
    /// </summary>
    public int GemCount = 0;

    /// <summary>
    /// 存档中存储的总矿物数量
    /// </summary>
    public int TotalOreCount = 0;
    
    /// <summary>
    /// 存档中存储的总宝石数量
    /// </summary>
    public int TotalGemCount = 0;

    /// <summary>
    /// 武器最大过热值
    /// </summary>
    public int MaxHeat = 100;
    
    /// <summary>
    /// 正常冷却速率：每秒降低的过热值
    /// </summary>
    public int ColdDownRateNormal = 5;
    
    /// <summary>
    /// 正常加热速率：每次射击增加的过热值
    /// </summary>
    public int ColdUpRateNormal = 2;
    
    /// <summary>
    /// 过热冷却速率：每秒降低的过热值
    /// </summary>
    public int ColdDownRateOverHeat = 20;

    /// <summary>
    /// 存档存储工具，用于加载和保存玩家数据
    /// </summary>
    private ISaveStorageUtility _saveStorageUtility = null!;

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 初始化单例实例，获取存档工具依赖，并从存档初始化总矿石量
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
        
        // 获取存档工具依赖
        _saveStorageUtility = this.GetUtility<ISaveStorageUtility>()!;
        
        // 从存档初始化总矿石量
        InitializeFromSave();
    }

    /// <summary>
    /// 从存档初始化总矿石量和总宝石量
    /// </summary>
    private void InitializeFromSave()
    {
        if (_saveStorageUtility != null)
        {
            TotalOreCount = _saveStorageUtility.GetItemCount("Ore");
            TotalGemCount = _saveStorageUtility.GetItemCount("Gem");
            GD.Print($"从存档初始化矿石量: Ore={TotalOreCount}, Gem={TotalGemCount}");
        }
    }

    /// <summary>
    /// 更新总矿石量和总宝石量
    /// 当任务矿石添加到存档后调用
    /// </summary>
    public void UpdateTotalOreFromSave()
    {
        if (_saveStorageUtility != null)
        {
            TotalOreCount = _saveStorageUtility.GetItemCount("Ore");
            TotalGemCount = _saveStorageUtility.GetItemCount("Gem");
        }
    }
}
