using Godot;
using System;
using System.Collections.Generic;
using CosmicMiningCompany.scripts.serializer;
using CosmicMiningCompany.scripts.data;

public partial class SpaceRock : RigidBody2D
{
	private Timer _lifeTimer;
	private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
	private Area2D ShootArea => GetNode<Area2D>("%ShootArea");
	private SpaceShip SpaceShip => GetTree().Root.GetNode<SpaceShip>("Space/SpaceShip");
	
	// 陨石属性，从JSON加载
	public AsteroidData AsteroidData { get; set; }

	// 添加标志来确保掉落只执行一次
	private bool _hasDroppedLoot = false;

	public override void _Ready()
	{
		LinearDamp = 0;
		AngularDamp = 0;
		ShootArea.BodyEntered += OnBodyEntered;
		this.LinearVelocity = new Vector2((float)GD.RandRange(-50, 50), (float)GD.RandRange(-50, 50));

		// 初始化计时器
		_lifeTimer = new Timer();
		_lifeTimer.WaitTime = 10.0f;
		_lifeTimer.OneShot = true;
		_lifeTimer.Timeout += () => QueueFree();
		AddChild(_lifeTimer);
	}

	public override void _PhysicsProcess(double delta)
	{
		DestroyRock();

	}
	

	private void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
			GD.Print("被子弹击中");
			// 减少血量
			if (AsteroidData != null)
			{
				AsteroidData.Health -= PlayerManager.Instance.Damage; // 每发子弹造成 n点伤害

				if (AsteroidData.Health <= 0 && !_hasDroppedLoot) // 确保只掉落一次
				{
					_hasDroppedLoot = true;
					DropLoot();
					var particlesNode = GetNodeOrNull<CpuParticles2D>("break/CPUParticles2D_break");
					var particlesNode2 = GetNodeOrNull<CpuParticles2D>("break/CPUParticles2D_blast");
					particlesNode.Emitting = true;
					particlesNode2.Emitting = true;
					var collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
					if (collisionShape != null)
					{
						collisionShape.SetDeferred("disabled", true); // 立即禁用碰撞形状
					}
					var animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
					if (animatedSprite != null)
					{
						animatedSprite.Visible = false;
                        ShootArea.QueueFree();
						Timer tempTimer = new Timer();
						tempTimer.WaitTime = 2.0f; // 粒子播放时长
						tempTimer.Timeout += () => QueueFree();
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

			// 向子弹发送信号，让它销毁自己
			bullet.DestroyBullet();
		}
	}

	private void DropLoot()
	{
		// 实现掉落逻辑
		if (AsteroidData != null)
		{
			GD.Print($"小行星掉落: {AsteroidData.Loot}");

			// 生成随机掉落数量 (1-3)
			int dropCount = (int)GD.RandRange(1, 4); // RandRange 是包含上限的

			for (int i = 0; i < dropCount; i++)
			{
				// 加载掉落物场景
				var lootScene = ResourceLoader.Load<PackedScene>("res://scenes/loot/loot.tscn");

				if (lootScene != null)
				{
					// 实例化掉落物
					var lootInstance = (Loot)lootScene.Instantiate();

					// 设置掉落物的位置（稍微分散一些）
					var offset = new Vector2((float)GD.RandRange(-20, 21), (float)GD.RandRange(-20, 21));
					lootInstance.GlobalPosition = this.GlobalPosition + offset;

					lootInstance.Initialize(AsteroidData.Loot);

					// 添加到场景树
					GetParent().AddChild(lootInstance);

					GD.Print($"生成掉落物: {AsteroidData.Loot} #{i + 1}");

				}
				else
				{
					GD.PrintErr("无法加载掉落物场景: res://scenes/loot/loot.tscn");
				}
			}
		}
	}

	public void DestroyRock()
	{
		float distance = (this.GlobalPosition - SpaceShip.Position).Length();

		if (distance > 3000)
		{
			// 如果计时器未启动，则启动它
			if (!_lifeTimer.IsStopped())
			{
				// 计时器已经在运行，无需操作
			}
			else
			{
				_lifeTimer.Start();
			}
		}
		else
		{
			// 如果计时器正在运行，则停止它
			if (!_lifeTimer.IsStopped())
			{
				_lifeTimer.Stop();
			}
		}
	}
}

// 为AsteroidData实现ISerializer接口
public class AsteroidJsonSerializer : ISerializer<AsteroidRoot>
{
	public string Serialize(AsteroidRoot data)
	{
		return Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
	}

	public AsteroidRoot Deserialize(string json)
	{
		return Newtonsoft.Json.JsonConvert.DeserializeObject<AsteroidRoot>(json) ?? new AsteroidRoot();
	}
}