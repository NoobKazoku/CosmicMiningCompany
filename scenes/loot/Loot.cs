using CosmicMiningCompany.scripts.core;
using CosmicMiningCompany.scripts.loot;
using Godot;
using System;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;

[Log]
[ContextAware]
public partial class Loot : CharacterBody2D, IPoolableNode, IController
{
	public const float Speed = 800.0f;
	public bool IsCollected = false;
	public string OreName = "";

	// 对象池引用
	private ILootPoolSystem _pool = null!;

	private SpaceShip SpaceShip => GetTree().Root.GetNode<SpaceShip>("Space/SpaceShip");
	private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
	private CollisionShape2D CollisionShape => GetNode<CollisionShape2D>("%CollisionShape2D");

	private Cargo Cargo => GetTree().Root.GetNode<Cargo>("Space/UI/Cargo");

	public override void _Ready()
	{
		_pool = this.GetSystem<ILootPoolSystem>()!;
	}


	public override void _PhysicsProcess(double delta)
	{
		if (IsCollected)
		{
			//应用速度，持续向飞船方向移动
			Vector2 direction = (SpaceShip.GlobalPosition - GlobalPosition).Normalized();
			Velocity = direction * Speed;
			MoveAndSlide();
		}

		if ((SpaceShip.GlobalPosition - GlobalPosition).Length() < 50)
		{
			Collected();
		}
	}

	/// <summary>
	/// 被飞船调用，修改状态为正在被拾取
	/// </summary>
	public void HasCollect()
	{
		GD.Print("矿石被吸引");
		IsCollected = true;
	}

	/// <summary>
	/// 播放拾取效果并回收资源到对象池
	/// </summary>
	public void Collected()
	{
		Cargo.CollectOre(OreName);
		RequestRecycle();
	}

	/// <summary>
	/// 请求回收（实现 IPoolableNode 接口）
	/// </summary>
	public void RequestRecycle()
	{
		_pool?.Release(this);
	}

	/// <summary>
	/// 从池中获取时调用
	/// </summary>
	public void OnAcquire()
	{
		IsCollected = false;
		Velocity = Vector2.Zero;
		Visible = true;
		SetProcess(true);
		SetPhysicsProcess(true);
		
		// 暂时禁用碰撞检测，避免刚生成时立即触发检测区域事件
		if (CollisionShape != null)
		{
			CollisionShape.Disabled = true;
			// 延迟一帧后重新启用碰撞检测
			CallDeferred(nameof(EnableCollision));
		}
	}
	
	/// <summary>
	/// 启用碰撞检测（延迟调用）
	/// </summary>
	private void EnableCollision()
	{
		if (CollisionShape != null)
		{
			CollisionShape.Disabled = false;
		}
	}

	/// <summary>
	/// 释放回池时调用
	/// </summary>
	public void OnRelease()
	{
		IsCollected = false;
		// 确保碰撞检测被禁用，避免在池中时仍然触发事件
		if (CollisionShape != null)
		{
			CollisionShape.Disabled = true;
		}
	}

	/// <summary>
	/// 池销毁时调用
	/// </summary>
	public void OnPoolDestroy()
	{
		// 清理资源
	}


	/// <summary>
	/// 设置对象池引用
	/// </summary>
	public void SetPool(ILootPoolSystem pool)
	{
		_pool = pool;
	}

	/// <summary>
	/// 初始化
	/// </summary>
	public void Initialize(string Name)
	{
		AnimatedSprite2D.Play(Name);
		OreName = Name;
	}
}
