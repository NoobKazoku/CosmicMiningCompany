using GFramework.Core.extensions;
using GFramework.Core.system;
using global::CosmicMiningCompany.global;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;


/// <summary>
/// 小行星生成系统，负责根据规则在指定位置生成小行星
/// </summary>
public class AsteroidSpawnSystem:AbstractSystem,IAsteroidSpawnSystem
{
    /// <summary>
    /// 小行星生成规则实例，用于决定生成条件和类型
    /// </summary>
    private AsteroidSpawnRule _rule = null!;
    
    /// <summary>
    /// 小行星工厂实例，用于创建小行星对象
    /// </summary>
    private AsteroidFactory _factory = null!;

    /// <summary>
    /// 尝试在指定位置生成小行星
    /// </summary>
    /// <param name="target">生成小行星的目标节点</param>
    /// <param name="spawnPosition">生成位置的向量坐标</param>
    public void TrySpawn(Node target,Vector2 spawnPosition)
    {
        var distance = spawnPosition.Length();

        // 检查是否满足生成条件
        if (!_rule.CanSpawn(distance))
            return;

        // 根据距离决定小行星ID并创建小行星对象
        var asteroidId = _rule.DecideAsteroidId(distance);
        var asteroid = _factory.Create(asteroidId, spawnPosition);

        // 将生成的小行星添加到目标节点
        target.AddChild(asteroid);
    }

    /// <summary>
    /// 初始化系统，设置数据读取工具、生成规则和工厂实例
    /// </summary>
    protected override void OnInit()
    {
        var dataReadUtility =  this.GetUtility<IAsteroidDataReadUtility>()!;
        var data = dataReadUtility.Current!;
        _rule = new AsteroidSpawnRule(data);
        _factory = new AsteroidFactory(data,AsteroidSceneRegistry.Instance);
    }
}
