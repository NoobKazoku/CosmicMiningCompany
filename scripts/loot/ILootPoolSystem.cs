using GFramework.Core.Abstractions.system;
using Godot;

namespace CosmicMiningCompany.scripts.loot;

/// <summary>
/// 掉落物对象池系统接口，用于管理掉落物对象的获取和回收
/// </summary>
public interface ILootPoolSystem : ISystem
{
    /// <summary>
    /// 从对象池中获取一个可用的掉落物对象
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <returns>返回获取到的 Loot 对象实例</returns>
    Loot Acquire(Node parent);
    
    /// <summary>
    /// 将使用完毕的掉落物对象归还到对象池中
    /// </summary>
    /// <param name="loot">需要回收的 Loot 对象实例</param>
    void Release(Loot loot);
}
