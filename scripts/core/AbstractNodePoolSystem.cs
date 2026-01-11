using System.Collections.Generic;
using GFramework.Core.system;
using Godot;

namespace CosmicMiningCompany.scripts.core;

/// <summary>
/// 抽象节点对象池系统，用于管理不同类型的可复用节点对象
/// </summary>
/// <typeparam name="TKey">用于标识不同节点类型的键类型</typeparam>
/// <typeparam name="TNode">继承自Node2D且实现IPoolableNode接口的节点类型</typeparam>
public abstract class AbstractNodePoolSystem<TKey, TNode>
    : AbstractSystem
    where TNode : Node2D, IPoolableNode where TKey : notnull
{
    /// <summary>
    /// 存储各类型节点池的字典，键为节点类型标识，值为该类型的节点栈
    /// </summary>
    protected readonly Dictionary<TKey, Stack<TNode>> Pools = new();

    /// <summary>
    /// 根据键加载对应类型的场景资源
    /// </summary>
    /// <param name="key">节点类型标识键</param>
    /// <returns>打包的场景资源对象</returns>
    protected abstract PackedScene LoadScene(TKey key);

    /// <summary>
    /// 从对象池中获取一个可用的节点实例
    /// </summary>
    /// <param name="key">节点类型标识键</param>
    /// <param name="parent">要将节点添加到的父节点</param>
    /// <returns>获取到的节点实例</returns>
    public TNode Acquire(TKey key, Node parent)
    {
        // 检查是否存在对应的节点池，不存在则创建新的池
        if (!Pools.TryGetValue(key, out var pool))
        {
            pool = new Stack<TNode>();
            Pools[key] = pool;
        }

        // 从池中取出节点或创建新节点
        TNode node = pool.Count > 0
            ? pool.Pop()
            : LoadScene(key).Instantiate<TNode>();

        parent.AddChild(node);
        Activate(node);
        node.OnAcquire();
        return node;
    }

    /// <summary>
    /// 将使用完毕的节点归还到对象池中
    /// </summary>
    /// <param name="key">节点类型标识键</param>
    /// <param name="node">需要归还的节点实例</param>
    public void Release(TKey key, TNode node)
    {
        node.OnRelease();
        Deactivate(node);
        node.GetParent()?.RemoveChild(node);

        // 检查是否存在对应的节点池，不存在则创建新的池
        if (!Pools.TryGetValue(key, out var pool))
        {
            pool = new Stack<TNode>();
            Pools[key] = pool;
        }

        pool.Push(node);
    }

    /// <summary>
    /// 清空所有节点池中的节点并释放资源
    /// </summary>
    public void Clear()
    {
        foreach (var pool in Pools.Values)
        {
            foreach (var node in pool)
            {
                node.OnPoolDestroy();
                node.QueueFree();
            }
        }
        Pools.Clear();
    }

    /// <summary>
    /// 激活节点，设置其可见性和处理状态
    /// </summary>
    /// <param name="node">需要激活的节点</param>
    protected virtual void Activate(TNode node)
    {
        node.Visible = true;
        node.SetProcess(true);
        node.SetPhysicsProcess(true);
        
        // 对于 RigidBody2D，需要恢复物理模拟
        if (node is RigidBody2D rigidBody)
        {
            rigidBody.Freeze = false;
        }
    }

    /// <summary>
    /// 停用节点，设置其不可见和停止处理状态
    /// </summary>
    /// <param name="node">需要停用的节点</param>
    protected virtual void Deactivate(TNode node)
    {
        node.Visible = false;
        node.SetProcess(false);
        node.SetPhysicsProcess(false);
        
        // 对于 RigidBody2D，需要停止物理模拟以避免性能损耗
        if (node is RigidBody2D rigidBody)
        {
            rigidBody.Freeze = true;
        }
    }

    /// <summary>
    /// 系统销毁时的清理操作
    /// </summary>
    protected override void OnDestroy()
    {
        Clear();
    }
}
