using System.Collections.Generic;
using GFramework.Core.system;
using global::CosmicMiningCompany.global;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星对象池系统，用于管理和复用小行星对象以提高性能
/// </summary>
public class AsteroidPoolSystem : AbstractSystem, IAsteroidPoolSystem
{
    /// <summary>
    /// 存储不同场景键对应的小行星对象池的字典
    /// </summary>
    private readonly Dictionary<string, Stack<scenes.space_rock.SpaceRock>> _pools = new();

    /// <summary>
    /// 从小行星池中获取一个可用的小行星对象
    /// </summary>
    /// <param name="sceneKey">场景键，用于标识不同类型的小行星</param>
    /// <param name="parent">父节点，新获取的小行星将被添加到此节点下</param>
    /// <returns>获取到的小行星对象</returns>
    public scenes.space_rock.SpaceRock Acquire(string sceneKey, Node2D parent)
    {
        // 检查是否存在对应的对象池，如果不存在则创建新的对象池
        if (!_pools.TryGetValue(sceneKey, out var pool))
        {
            pool = new Stack<scenes.space_rock.SpaceRock>();
            _pools[sceneKey] = pool;
        }

        scenes.space_rock.SpaceRock rock;

        // 如果对象池中有可用对象，则从池中取出；否则创建新对象
        if (pool.Count > 0)
        {
            rock = pool.Pop();
        }
        else
        {
            var scene = AsteroidSceneRegistry.Instance.Get(sceneKey);
            rock = scene.Instantiate<scenes.space_rock.SpaceRock>();
        }

        parent.AddChild(rock);
        Activate(rock);

        return rock;
    }

    /// <summary>
    /// 将使用完毕的小行星对象归还到对象池中
    /// </summary>
    /// <param name="sceneKey">场景键，用于标识小行星类型</param>
    /// <param name="rock">需要归还的小行星对象</param>
    public void Release(string sceneKey, scenes.space_rock.SpaceRock rock)
    {
        Deactivate(rock);
        rock.GetParent()?.RemoveChild(rock);

        // 检查是否存在对应的对象池，如果不存在则创建新的对象池
        if (!_pools.TryGetValue(sceneKey, out var pool))
        {
            pool = new Stack<scenes.space_rock.SpaceRock>();
            _pools[sceneKey] = pool;
        }

        pool.Push(rock);
    }

    /// <summary>
    /// 激活小行星对象，使其可见并启用处理逻辑
    /// </summary>
    /// <param name="rock">需要激活的小行星对象</param>
    private static void Activate(scenes.space_rock.SpaceRock rock)
    {
        rock.Visible = true;
        rock.SetProcess(true);
        rock.SetPhysicsProcess(true);
    }

    /// <summary>
    /// 停用小行星对象，使其不可见并禁用处理逻辑
    /// </summary>
    /// <param name="rock">需要停用的小行星对象</param>
    private static void Deactivate(scenes.space_rock.SpaceRock rock)
    {
        rock.Visible = false;
        rock.SetProcess(false);
        rock.SetPhysicsProcess(false);
    }

    /// <summary>
    /// 初始化系统时调用的方法
    /// </summary>
    protected override void OnInit()
    {
        
    }
}
