using System.Collections.Generic;
using System.Linq;
using GFramework.SourceGenerators.Abstractions.logging;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星工厂类，负责根据定义创建小行星实例
/// </summary>
[Log]
public partial class AsteroidFactory
{
    /// <summary>
    /// 小行星定义字典，以ID为键存储小行星定义
    /// </summary>
    private readonly Dictionary<int, AsteroidDefinition> _defs;
    
    /// <summary>
    /// 小行星场景注册表，用于获取小行星场景资源
    /// </summary>
    private readonly IAsteroidSceneRegistry _sceneRegistry;

    /// <summary>
    /// 构造函数，初始化小行星工厂
    /// </summary>
    /// <param name="data">包含小行星定义的数据对象</param>
    /// <param name="sceneRegistry">小行星场景注册表</param>
    public AsteroidFactory(
        AsteroidData data,
        IAsteroidSceneRegistry sceneRegistry)
    {
        _sceneRegistry = sceneRegistry;
        _defs = data.Definitions.ToDictionary(d => d.Id);
    }

    /// <summary>
    /// 创建一个小行星实例
    /// </summary>
    /// <param name="asteroidId">要创建的小行星ID</param>
    /// <param name="pos">小行星在场景中的位置</param>
    /// <returns>创建的小行星节点</returns>
    public Node2D Create(int asteroidId, Vector2 pos)
    {
        var def = _defs[asteroidId];
        var scene = _sceneRegistry.Get(def.SceneKey);

        var node = scene.Instantiate<Node2D>();
        node.Position = pos;
        _log.Debug($"Created asteroid {asteroidId},Position:{pos}");

        if (node is IAsteroid asteroid)
        {
            asteroid.Init(def.BaseHealth, def.Loot);
        }

        return node;
    }
}
