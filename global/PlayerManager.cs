using System;
using Godot;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;

[ContextAware]
[Log]
public partial class PlayerManager : Node
{
    public static PlayerManager Instance { get; private set; }
    
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

    public int MaxHeat = 100; //最大过热值
    public int ColdDownRateNormal = 5; // 正常冷却速率：每秒降低 n点
    public int ColdUpRateNormal = 2; // 正常加热速率：每次射击增加 n点
    public int ColdDownRateOverHeat = 20; // 过热冷却速率：每秒降低 n点

    public override void _Ready()
    {
        Instance = this;
    }
}