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
    public static PlayerManager? Instance { get; private set; }
    
    public float MaxFuel = 100.0f;//最大燃料
    public float FuelConsumptionRate = 0.333f; // 每秒消耗的燃料量

    public int WeaponCount = 2; // 武器数量
    public int Damage = 5; // 子弹伤害
    public float FireRate = 0.2f; // 射击间隔，单位秒

    public float Speed = 300.0f; // 最大速度
    public float Acceleration = 200.0f; // 加速度
    public float BrakeForce = 150.0f; // 制动力
    public float RotationSpeed = 5.0f; //转向角速度

    public int OreGet = 10;	//矿物获取量
    public int PickupRange = 1;//拾取范围大小倍率

    //任务中获得的矿石数量
    public int OreCount = 0; //任务获得散矿数量
    public int GemCount = 0; //任务获得宝石数量

    //存档的矿物总量
    public int TotalOreCount = 0; //总矿物数量
    public int TotalGemCount = 0; //总宝石数量

    public int MaxHeat = 100; //最大过热值
    public int ColdDownRateNormal = 5; // 正常冷却速率：每秒降低 n点
    public float ColdUpRateNormal = 2.0f; // 正常加热速率：每次射击增加 n点
    public int ColdDownRateOverHeat = 20; // 过热冷却速率：每秒降低 n点

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
