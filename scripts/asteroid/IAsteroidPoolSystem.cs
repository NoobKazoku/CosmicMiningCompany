using CosmicMiningCompany.scenes.space_rock;
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
    /// <param name="sceneKey">场景键值，用于标识特定的小行星场景</param>
    /// <param name="parent">父节点，指定小行星对象的父级容器</param>
    /// <returns>返回获取到的SpaceRock对象实例</returns>
    SpaceRock Acquire(string sceneKey, Node2D parent);
    
    /// <summary>
    /// 将使用完毕的小行星对象归还到对象池中
    /// </summary>
    /// <param name="sceneKey">场景键值，用于标识特定的小行星场景</param>
    /// <param name="rock">需要回收的SpaceRock对象实例</param>
    void Release(string sceneKey, SpaceRock rock);
}

