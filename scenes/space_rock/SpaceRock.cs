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
		//读取json文件，初始化陨石属性
		InitializeAsteroidProperties();

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

	private void InitializeAsteroidProperties()
	{
		// 读取Asteroid.json文件内容
		string jsonFilePath = "res://assets/data/Asteroid.json";

		if (!FileAccess.FileExists(jsonFilePath))
		{
			GD.PrintErr($"文件不存在: {jsonFilePath}");
			return;
		}

		try
		{
			// 使用FileAccess读取文件内容
			string jsonString = FileAccess.GetFileAsString(jsonFilePath);

			// 使用ISerializer接口反序列化JSON
			var serializer = new AsteroidJsonSerializer();
			var asteroidRoot = serializer.Deserialize(jsonString);

			// 使用AsteroidDataHelper获取实际数据
			var actualData = AsteroidDataHelper.GetActualAsteroidData(asteroidRoot);

			if (actualData.Count > 0)
			{
				// 根据刷新概率系数随机选择一个陨石数据
				AsteroidData = SelectAsteroidByProbability(actualData);

				GD.Print($"小行星加载成功: {AsteroidData.Name} (ID: {AsteroidData.Id}, 血量: {AsteroidData.Health}, 概率系数: {AsteroidData.Probability})");

				// 初始化

				// 设置动画
				if (AnimatedSprite2D != null)
				{
					AnimatedSprite2D.Play(AsteroidData.Name);
				}
			}
			else
			{
				GD.PrintErr("没有找到有效的陨石数据");
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"读取或解析Asteroid.json时出错: {e.Message}");
		}
	}

	private AsteroidData SelectAsteroidByProbability(List<AsteroidData> asteroids)
	{
		// 使用概率系数来选择陨石
		int totalProbability = 0;
		foreach (var asteroid in asteroids)
		{
			totalProbability += asteroid.Probability;
		}

		// 生成一个随机数
		int randomValue = GD.RandRange(0, totalProbability);

		// 根据概率选择陨石
		int currentProbability = 0;
		foreach (var asteroid in asteroids)
		{
			currentProbability += asteroid.Probability;
			if (randomValue <= currentProbability)
			{
				return asteroid;
			}
		}

		// 如果没有找到匹配的陨石，返回第一个
		return asteroids[0];
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
			GD.Print("被子弹击中");
			// 减少血量
			if (AsteroidData != null)
			{
				AsteroidData.Health -= 5; // 每发子弹造成5点伤害

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