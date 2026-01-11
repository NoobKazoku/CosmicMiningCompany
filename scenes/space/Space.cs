using System.Collections.Generic;
using CosmicMiningCompany.scripts.asteroid;
using CosmicMiningCompany.scripts.enums;
using CosmicMiningCompany.scripts.events.audio;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.space;

/// <summary>
/// 太空场景节点，负责管理陨石生成、销毁和太空船相关逻辑
/// </summary>
[ContextAware]
[Log]
public partial class Space :Node2D,IController
{
	private IAsteroidSpawnSystem _asteroidSpawnSystem = null!;
	private Timer _spawnTimer = null!;
	
	/// <summary>
	/// 获取陨石根节点
	/// </summary>
	private Node2D AsteroidRoot => GetNode<Node2D>("%AsteroidRoot");
	
	/// <summary>
	/// 获取2D相机节点
	/// </summary>
	private Camera2D Camera => GetNode<Camera2D>("%Camera2D");
	
	/// <summary>
	/// 获取太空船节点
	/// </summary>
	private CharacterBody2D SpaceShip => GetNode<CharacterBody2D>("%SpaceShip");
	
	/// <summary>
	/// 每次生成的陨石数量
	/// </summary>
	[Export]
	private int _spawnCountPerTick = 3;
	
	/// <summary>
	/// 陨石销毁距离阈值
	/// </summary>
	[Export]
	private float _despawnDistance = 5000f;
	
	/// <summary>
	/// 陨石被标记为需要销毁后的额外等待时间（秒）
	/// </summary>
	[Export]
	private float _despawnGraceTime = 5f;
	
	/// <summary>
	/// 记录每个陨石被标记为需要销毁的时间
	/// </summary>
	private Dictionary<space_rock.SpaceRock, float> _markedForDestroyTimes = new();
	
	/// <summary>
	/// 物理处理回调方法
	/// 检查所有陨石与太空船的距离，超出阈值的陨石将被标记销毁
	/// </summary>
	/// <param name="delta">物理帧时间间隔</param>
	public override void _PhysicsProcess(double delta)
	{
		// 平滑跟随太空船
		float followSpeed = 2.0f; // 跟随速度，值越大跟随越快
		Camera.GlobalPosition = Camera.GlobalPosition.Lerp(SpaceShip.GlobalPosition, (float)delta * followSpeed);

		var viewRect = GetCameraWorldRect();
		var viewExpansion = 500f; // 视野外多少距离开始计时销毁

		foreach (var rock in AsteroidRoot.GetChildren())
		{
			if (rock is not space_rock.SpaceRock spaceRock)
				continue;

			// 检查陨石是否在视野内（在视野内则不销毁）
			if (viewRect.HasPoint(spaceRock.GlobalPosition))
			{
				_markedForDestroyTimes.Remove(spaceRock);
				continue;
			}

			// 检查陨石是否在扩展视野外
			var expandedRect = viewRect.Expand(viewRect.Position - new Vector2(viewExpansion, viewExpansion));
			expandedRect = expandedRect.Expand(viewRect.End + new Vector2(viewExpansion, viewExpansion));
			
			if (!expandedRect.HasPoint(spaceRock.GlobalPosition))
			{
				// 标记需要销毁，并记录时间
				if (!_markedForDestroyTimes.ContainsKey(spaceRock))
				{
					_markedForDestroyTimes[spaceRock] = (float)Time.GetTicksMsec() / 1000f;
				}
				else
				{
					var markedTime = _markedForDestroyTimes[spaceRock];
					var currentTime = (float)Time.GetTicksMsec() / 1000f;
					
					// 如果标记时间已经超过宽限期，触发销毁
					if (currentTime - markedTime > _despawnGraceTime)
					{
						_markedForDestroyTimes.Remove(spaceRock);
						spaceRock.RequestRecycle();
					}
				}
			}
			else
			{
				// 陨石回到扩展视野内，移除标记
				_markedForDestroyTimes.Remove(spaceRock);
			}
		}
	}
	
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		_asteroidSpawnSystem = this.GetSystem<IAsteroidSpawnSystem>()!;
		_spawnTimer = new Timer
		{
			WaitTime = 0.5f,
			OneShot = false,
			Autostart = true
		};
		_spawnTimer.Timeout += OnSpawnTimeout;
		AddChild(_spawnTimer);
		this.SendEvent(new BgmChangedEvent()
		{
			BgmType = BgmType.Gaming
		});
	}
	
	/// <summary>
	/// 生成计时器超时回调方法
	/// 根据配置数量在视野外生成陨石
	/// </summary>
	private void OnSpawnTimeout()
	{
		for (int i = 0; i < _spawnCountPerTick; i++)
		{
			var spawnPosition = GetRandomSpawnPositionOutsidePlayerView();
			_asteroidSpawnSystem.TrySpawn(AsteroidRoot, spawnPosition, SpaceShip.GlobalPosition);
		}
	}
	
	/// <summary>
	/// 在玩家视野范围外生成随机位置
	/// </summary>
	/// <returns>视野外的随机生成位置</returns>
	private Vector2 GetRandomSpawnPositionOutsidePlayerView()
	{
		var view = GetCameraWorldRect();
		const float margin = 150f; // 刷在视野外多远

		var side = GD.RandRange(0, 3);

		return side switch
		{
			// 上
			0 => new Vector2(
				(float)GD.RandRange(view.Position.X, view.End.X),
				view.Position.Y - margin),

			// 下
			1 => new Vector2(
				(float)GD.RandRange(view.Position.X, view.End.X),
				view.End.Y + margin),

			// 左
			2 => new Vector2(
				view.Position.X - margin,
				(float)GD.RandRange(view.Position.Y, view.End.Y)),

			// 右
			_ => new Vector2(
				view.End.X + margin,
				(float)GD.RandRange(view.Position.Y, view.End.Y))
		};
	}

	/// <summary>
	/// 获取相机的世界矩形范围
	/// </summary>
	/// <returns>相机覆盖的世界矩形</returns>
	private Rect2 GetCameraWorldRect()
	{
		var viewportSize = GetViewportRect().Size;
		var zoom = Camera.Zoom;

		var size = viewportSize * zoom;
		var topLeft = Camera.GlobalPosition - size / 2f;

		return new Rect2(topLeft, size);
	}

}
