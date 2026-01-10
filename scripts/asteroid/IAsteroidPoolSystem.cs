using GFramework.Core.Abstractions.system;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星对象池系统接口，用于管理小行星对象的获取和回收
/// </summary>
public interface IAsteroidPoolSystem : ISystem
{
    /// <summary>
    /// 从小行星对象池中获取一个可用的小行星对象
    /// </summary>
    /// <param name="parent">要将获取的小行星添加到的父节点</param>
    /// <returns>获取的SpaceRock对象实例</returns>
    SpaceRock Acquire(Node2D parent);
    
    /// <summary>
    /// 将使用完毕的小行星对象归还到对象池中
    /// </summary>
    /// <param name="rock">需要归还的小行星对象</param>
    void Release(SpaceRock rock);
}
