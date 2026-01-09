namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 表示小行星生成规则项，定义了特定类型小行星的生成参数和权重计算规则
/// </summary>
public class AsteroidSpawnRuleItem
{
    /// <summary>
    /// 小行星的唯一标识符ID
    /// </summary>
    public int AsteroidId { get; set; }

    /// <summary>
    /// 小行星生成的最小距离限制
    /// </summary>
    public float MinDistance { get; set; }
    
    /// <summary>
    /// 小行星生成的最大距离限制
    /// </summary>
    public float MaxDistance { get; set; }

    /// <summary>
    /// 小行星的基础权重值，用于生成概率计算
    /// </summary>
    public float BaseWeight { get; set; }
    
    /// <summary>
    /// 距离因子，用于根据距离调整生成权重
    /// </summary>
    public float DistanceFactor { get; set; }
}
