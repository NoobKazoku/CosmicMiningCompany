using System.Collections.Generic;
using CosmicMiningCompany.scripts.core;
using GFramework.Core.system;
using Godot;

namespace CosmicMiningCompany.scripts.loot;

/// <summary>
/// 简单节点对象池，不使用泛型键
/// </summary>
public class SimpleNodePool<TNode> : AbstractSystem where TNode : Node2D, IPoolableNode
{
    protected readonly Stack<TNode> Pool = new();
    protected PackedScene? _scene;
    
    protected virtual PackedScene LoadScene()
    {
        return _scene!;
    }
    
    public TNode Acquire(Node parent)
    {
        TNode node = Pool.Count > 0
            ? Pool.Pop()
            : LoadScene().Instantiate<TNode>();
        
        parent.AddChild(node);
        node.SetProcess(true);
        node.SetPhysicsProcess(true);
        node.Visible = true;
        
        // 对于 CharacterBody2D/RigidBody2D，恢复物理模拟
        if (node is CharacterBody2D characterBody)
        {
            // CharacterBody2D 使用 MotionMode，没有 Freeze，但可以通过设置速度为0来停止
            characterBody.Velocity = Vector2.Zero;
        }
        else if (node is RigidBody2D rigidBody)
        {
            rigidBody.Freeze = false;
        }
        
        node.OnAcquire();
        
        return node;
    }
    
    public void Release(TNode node)
    {
        node.OnRelease();
        node.SetProcess(false);
        node.SetPhysicsProcess(false);
        node.Visible = false;
        
        // 对于 CharacterBody2D/RigidBody2D，停止物理模拟
        if (node is CharacterBody2D characterBody)
        {
            // CharacterBody2D 通过停止速度和禁用物理处理来停止
            characterBody.Velocity = Vector2.Zero;
        }
        else if (node is RigidBody2D rigidBody)
        {
            rigidBody.Freeze = true;
        }
        
        node.GetParent()?.RemoveChild(node);
        
        Pool.Push(node);
    }
    
    protected override void OnInit()
    {
        // 初始化
    }
    
    protected override void OnDestroy()
    {
        foreach (var node in Pool)
        {
            node.OnPoolDestroy();
            node.QueueFree();
        }
        Pool.Clear();
    }
}

/// <summary>
/// 掉落物对象池系统，用于管理和复用掉落物对象以提高性能
/// </summary>
public class LootPoolSystem : SimpleNodePool<Loot>, ILootPoolSystem
{
    private static readonly PackedScene LootScene = ResourceLoader.Load<PackedScene>("res://scenes/loot/loot.tscn");
    
    protected override PackedScene LoadScene()
    {
        return LootScene;
    }
    
    public new Loot Acquire(Node parent)
    {
        return base.Acquire(parent);
    }
    
    public new void Release(Loot loot)
    {
        base.Release(loot);
    }
}
