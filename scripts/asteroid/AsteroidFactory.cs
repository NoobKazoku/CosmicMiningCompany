using System.Collections.Generic;
using System.Linq;
using GFramework.SourceGenerators.Abstractions.logging;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星工厂类，负责根据定义创建小行星实例
/// </summary>
[Log]
public partial class AsteroidFactory
{
    private readonly Dictionary<int, AsteroidDefinition> _defs;

    /// <summary>
    /// 初始化小行星工厂实例
    /// </summary>
    /// <param name="defs">包含小行星定义数据的数据对象</param>
    public AsteroidFactory(Dictionary<int, AsteroidDefinition> defs)
    {
        // 将小行星定义数据转换为以ID为键的字典，便于快速查找
        _defs = defs;
    }

    /// <summary>
    /// 根据指定的小行星ID配置空间岩石对象
    /// </summary>
    /// <param name="rock">需要被配置的空间岩石对象</param>
    /// <param name="asteroidId">用于查找小行星定义的小行星ID</param>
    public void Configure(scenes.space_rock.SpaceRock rock, int asteroidId)
    {
        // 根据ID从字典中获取对应的小行星定义
        var def = _defs[asteroidId];

        // 使用小行星定义中的属性初始化空间岩石对象
        rock.Init(def);
    }
}

