using CosmicMiningCompany.scripts.asteroid;
using Godot;
using AsteroidData = CosmicMiningCompany.scripts.data.AsteroidData;

public partial class SpaceRock : RigidBody2D,IAsteroid
{
	private Timer _lifeTimer;
	private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
	private Area2D ShootArea => GetNode<Area2D>("%ShootArea");
	
	// 陨石属性，从JSON加载
	public AsteroidData AsteroidData { get; set; }

	// 添加标志来确保掉落只执行一次
	private bool _hasDroppedLoot;

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
	
	public void Init(float hp, string dropId, string asteroidName)
	{
		// 初始化AsteroidData
		AsteroidData = new AsteroidData
		{
			Health = hp,
			Loot = dropId
		};
		
		// 播放对应的陨石动画
		if (AnimatedSprite2D != null)
		{
			AnimatedSprite2D.Play(asteroidName);
		}
		
		GD.Print($"陨石初始化: 血量={hp}, 掉落物={dropId}, 名称={asteroidName}");
	}
	private void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
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

			// 生成随机掉落数量 (1-4)
			int dropCount = (int)GD.RandRange(1, 4); 

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

	public void ScheduleDestroy(float delay)
	{
		var timer = new Timer
		{
			WaitTime = delay,
			OneShot = true
		};
		timer.Timeout += QueueFree;
		AddChild(timer);
		timer.Start();
	}
}
