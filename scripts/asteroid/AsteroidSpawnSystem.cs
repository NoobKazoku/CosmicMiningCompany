using GFramework.Core.extensions;
using GFramework.Core.system;
using GFramework.SourceGenerators.Abstractions.logging;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星生成系统，负责根据规则在指定位置生成小行星
/// </summary>
[Log]
public partial class AsteroidSpawnSystem : AbstractSystem, IAsteroidSpawnSystem
{
    /// <summary>
    /// 小行星生成规则实例，用于决定生成条件和类型
    /// </summary>
    private AsteroidSpawnRule _rule = null!;

    /// <summary>
    /// 小行星工厂实例，用于创建小行星对象
    /// </summary>
    private AsteroidFactory _factory = null!;

    private IAsteroidPoolSystem _pool = null!;

    private IAsteroidDataReadUtility _dataReadUtility = null!;

    /// <summary>
    /// 尝试在指定位置生成小行星
    /// </summary>
    /// <param name="target">生成小行星的目标节点</param>
    /// <param name="spawnPosition">生成位置的向量坐标</param>
    /// <param name="origin">生成位置的向量坐标</param>
    public void TrySpawn(Node target, Vector2 spawnPosition, Vector2 origin)
    {
        var distance = spawnPosition.DistanceTo(origin);
        _log.Debug($"Checking spawn condition for distance {distance}");

        if (!_rule.CanSpawn(distance))
        {
            _log.Debug("Spawn condition not met.");
            return;
        }

        var asteroidId = _rule.DecideAsteroidId(distance);
        _log.Debug($"Decided asteroid ID: {asteroidId}");

        if (asteroidId < 0)
        {
            _log.Debug("Invalid asteroid ID.");
            return;
        }

        var def = _dataReadUtility.GetAsteroidDefs()[asteroidId];
        var rock = _pool.Acquire(def.SceneKey, (Node2D)target);
        rock.GlobalPosition = spawnPosition;
        _factory.Configure(rock, asteroidId);
        _log.Debug($"Asteroid spawned at {spawnPosition}");
    }


    /// <summary>
    /// 初始化系统，设置数据读取工具、生成规则和工厂实例
    /// </summary>
    protected override void OnInit()
    {
        _dataReadUtility = this.GetUtility<IAsteroidDataReadUtility>()!;
        var data = _dataReadUtility.Current!;
        _rule = new AsteroidSpawnRule(data);
        _factory = new AsteroidFactory(_dataReadUtility.GetAsteroidDefs());
        _pool = this.GetSystem<IAsteroidPoolSystem>()!;
    }
}