using System.Collections.Generic;
using GFramework.Core.system;
using Godot;

namespace CosmicMiningCompany.scripts.core;

/// <summary>
/// 抽象节点对象池基类，用于管理和复用Node2D类型的节点对象
/// </summary>
/// <typeparam name="T">继承自Node2D的具体节点类型</typeparam>
public abstract class AbstractNodePool<T>
    : AbstractSystem, INodePool<T>
    where T : Node2D
{
    private readonly Stack<T> _pool = new();
    private PackedScene _scene = null!;

    /// <summary>
    /// 初始化系统时加载场景资源
    /// </summary>
    protected override void OnInit()
    {
        _scene = LoadScene();
    }

    /// <summary>
    /// 加载节点场景的抽象方法，子类必须实现
    /// </summary>
    /// <returns>打包的场景资源</returns>
    protected abstract PackedScene LoadScene();

    /// <summary>
    /// 获取一个可用的节点实例
    /// </summary>
    /// <param name="parent">父节点容器</param>
    /// <returns>可用的节点实例</returns>
    public virtual T Acquire(Node parent)
    {
        var node = _pool.Count > 0
            ? _pool.Pop()
            : _scene.Instantiate<T>();

        parent.AddChild(node);
        Activate(node);

        return node;
    }

    /// <summary>
    /// 释放节点实例回对象池
    /// </summary>
    /// <param name="node">需要释放的节点实例</param>
    public virtual void Release(T node)
    {
        Deactivate(node);
        node.GetParent()?.RemoveChild(node);
        _pool.Push(node);
    }

    /// <summary>
    /// 激活节点使其可见并启用处理逻辑
    /// </summary>
    /// <param name="node">需要激活的节点</param>
    protected virtual void Activate(T node)
    {
        node.Visible = true;
        node.SetProcess(true);
        node.SetPhysicsProcess(true);
    }

    /// <summary>
    /// 停用节点使其不可见并禁用处理逻辑
    /// </summary>
    /// <param name="node">需要停用的节点</param>
    protected virtual void Deactivate(T node)
    {
        node.Visible = false;
        node.SetProcess(false);
        node.SetPhysicsProcess(false);
    }

    /// <summary>
    /// 获取当前对象池中可用节点的数量
    /// </summary>
    public int Count => _pool.Count;
    
    /// <summary>
    /// 获取对象池的容量（与当前数量相同）
    /// </summary>
    public virtual int Capacity => _pool.Count;
    
    /// <summary>
    /// 清空对象池中的所有节点
    /// </summary>
    public virtual void Clear() => _pool.Clear();
    
    /// <summary>
    /// 初始化对象池到指定容量
    /// </summary>
    /// <param name="initialCapacity">初始容量</param>
    public virtual void Initialize(int initialCapacity) => Preload(initialCapacity);

    /// <summary>
    /// 预加载指定数量的节点到对象池中
    /// </summary>
    /// <param name="count">预加载的数量</param>
    public virtual void Preload(int count)
    {
        for (int i = 0; i < count; i++)
            _pool.Push(_scene.Instantiate<T>());
    }

    /// <summary>
    /// 尝试获取一个可用的节点实例
    /// </summary>
    /// <param name="item">输出的节点实例</param>
    /// <returns>如果成功获取则返回true，否则返回false</returns>
    public bool TryAcquire(out T item)
    {
        if (_pool.Count == 0)
        {
            item = null!;
            return false;
        }

        item = _pool.Pop();
        Activate(item);
        return true;
    }


    /// <summary>
    /// 尝试获取一个可用的节点实例并添加到指定父节点
    /// </summary>
    /// <param name="parent">父节点容器</param>
    /// <param name="item">输出的节点实例</param>
    /// <returns>如果成功获取则返回true，否则返回false</returns>
    public bool TryAcquire(Node parent, out T item)
    {
        if (_pool.Count == 0)
        {
            item = null!;
            return false;
        }

        item = Acquire(parent);
        return true;
    }
}
