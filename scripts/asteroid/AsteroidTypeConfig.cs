using Godot;
using CosmicMiningCompany.scripts.enums;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星类型配置资源类，用于定义不同类型小行星的基础属性和配置信息
/// </summary>
[GlobalClass]
public partial class AsteroidTypeConfig : Resource
{
    /// <summary>
    /// 小行星类型枚举值
    /// </summary>
    [Export] public AsteroidType Type;
    
    /// <summary>
    /// 小行星的基础生命值，默认为10f
    /// </summary>
    [Export] public float BaseHp = 10f;
    
    /// <summary>
    /// 小行星对应的场景预制体，用于实例化小行星对象
    /// </summary>
    [Export] public required PackedScene Scene { get; set; }
    
    /// <summary>
    /// 小行星被破坏后掉落物品的ID标识符
    /// </summary>
    [Export] public required string DropId { get; set; }
}
