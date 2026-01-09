namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星定义类，用于描述小行星的基本属性和配置信息
/// </summary>
public class AsteroidDefinition
{
    /// <summary>
    /// 小行星的唯一标识符
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 小行星的名称
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// 小行星的基础生命值
    /// </summary>
    public int BaseHealth { get; set; }
    
    /// <summary>
    /// 小行星被破坏后掉落的战利品信息
    /// </summary>
    public required string Loot { get; set; }

    /// <summary>用于 Factory 映射场景</summary>
    public required string SceneKey { get; set; }
}
