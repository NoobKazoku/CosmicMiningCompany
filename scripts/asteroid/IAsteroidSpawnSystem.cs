using GFramework.Core.Abstractions.system;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星生成系统接口，继承自系统抽象接口
/// 定义了小行星生成系统的基本操作规范
/// </summary>
public interface IAsteroidSpawnSystem: ISystem
{
    /// <summary>
    /// 尝试在指定位置生成小行星
    /// </summary>
    /// <param name="target">小行星生成的目标节点</param>
    /// <param name="spawnPosition">小行星生成的位置坐标</param>
    /// <param name="origin">小行星生成的原点坐标</param>
    public void TrySpawn(Node target, Vector2 spawnPosition, Vector2 origin);
}
