using GFramework.Core.system;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;


/// <summary>
/// 小行星生成系统，负责根据规则在指定位置生成小行星
/// </summary>
/// <param name="data">小行星数据，包含定义和生成规则</param>
/// <param name="worldRoot">世界根节点，用于添加生成的小行星节点</param>
public class AsteroidSpawnSystem(
    AsteroidData data,
    Node worldRoot):AbstractSystem,IAsteroidSpawnSystem
{
    /// <summary>
    /// 小行星生成规则实例，用于决定生成条件和类型
    /// </summary>
    private readonly AsteroidSpawnRule _rule = new(data);
    
    /// <summary>
    /// 小行星工厂实例，用于创建小行星对象
    /// </summary>
    private readonly AsteroidFactory _factory = new(data, AsteroidSceneRegistry.Instance);

    /// <summary>
    /// 尝试在指定位置生成小行星
    /// </summary>
    /// <param name="spawnPosition">生成位置的向量坐标</param>
    public void TrySpawn(Vector2 spawnPosition)
    {
        var distance = spawnPosition.Length();

        if (!_rule.CanSpawn(distance))
            return;

        var asteroidId = _rule.DecideAsteroidId(distance);
        var asteroid = _factory.Create(asteroidId, spawnPosition);

        worldRoot.AddChild(asteroid);
    }

    protected override void OnInit()
    {
        
    }
}
