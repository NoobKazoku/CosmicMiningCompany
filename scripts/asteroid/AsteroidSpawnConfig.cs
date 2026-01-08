using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星生成配置资源类，用于定义小行星和宝石小行星的生成距离和权重参数
/// </summary>
[GlobalClass]
public partial class AsteroidSpawnConfig : Resource
{
    /// <summary>
    /// 普通小行星的起始生成距离
    /// </summary>
    [Export] public float NormalStartDistance = 500f;

    /// <summary>
    /// 宝石小行星的起始生成距离
    /// </summary>
    [Export] public float GemStartDistance = 1500f;

    /// <summary>
    /// 宝石小行星的权重因子，用于计算生成概率
    /// </summary>
    [Export] public float GemWeightFactor = 0.001f;

    /// <summary>
    /// 宝石小行星的最大权重值
    /// </summary>
    [Export] public float GemMaxWeight = 3f;
}

