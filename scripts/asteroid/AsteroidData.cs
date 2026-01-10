using System.Collections.Generic;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星数据类，用于存储小行星相关数据集合
/// </summary>
public class AsteroidData
{
    /// <summary>
    /// 小行星定义列表，存储所有小行星的定义信息
    /// </summary>
    public List<AsteroidDefinition> Definitions { get; set; } = [];
    
    /// <summary>
    /// 小行星生成规则列表，存储小行星的生成规则配置
    /// </summary>
    public List<AsteroidSpawnRuleItem> SpawnRules { get; set; } = [];
}
