using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
	[Export] public float BulletSpeed = 800.0f;
	private Timer _lifeTimer;


	public override void _Ready()
	{
		// 创建并启动计时器，3秒后销毁子弹
		_lifeTimer = new Timer();
		_lifeTimer.WaitTime = 3.0f;
		_lifeTimer.Timeout += () => QueueFree(); // 3秒后销毁子弹
		_lifeTimer.OneShot = true;
		AddChild(_lifeTimer);
		_lifeTimer.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}


	/// <summary>
	/// 被陨石调用，销毁子弹
	/// </summary>
	public void DestroyBullet()
	{
		Velocity = Vector2.Zero;
		// 停止计时器
		if (_lifeTimer != null)
		{
			_lifeTimer.Stop();
		}
		var particlesNode = GetNodeOrNull<CpuParticles2D>("CPUParticles2D_hit");
		if (particlesNode != null)
		{

			particlesNode.Emitting = true;
			var collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
			if (collisionShape != null)
			{
				collisionShape.SetDeferred("disabled", true); // 立即禁用碰撞形状
			}
			var animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
			if (animatedSprite != null)
			{
				animatedSprite.Visible = false; // 隐藏子弹图层
			}

			// 创建临时计时器，在粒子效果播放完毕后销毁子弹
			Timer tempTimer = new Timer();
			tempTimer.WaitTime = 0.6f; // 粒子播放时长
			tempTimer.Timeout += () => QueueFree(); // 0.6秒后销毁子弹
			tempTimer.OneShot = true;
			AddChild(tempTimer);
			tempTimer.Start();

		}
		else
		{
			QueueFree();
		}
	}
}
