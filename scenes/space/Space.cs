using CosmicMiningCompany.scripts.asteroid;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.space;

[ContextAware]
[Log]
public partial class Space :Node2D,IController
{
	private IAsteroidSpawnSystem _asteroidSpawnSystem = null!;
	private Timer _spawnTimer = null!;
	private Node2D AsteroidRoot => GetNode<Node2D>("%AsteroidRoot");
	private Camera2D Camera => GetNode<Camera2D>("%Camera2D");
	private CharacterBody2D SpaceShip => GetNode<CharacterBody2D>("%SpaceShip");
	/// <summary>
	/// 每次生成的陨石数量
	/// </summary>
	[Export]
	private int _spawnCountPerTick = 3;

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
	}
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

	private Rect2 GetCameraWorldRect()
	{
		var viewportSize = GetViewportRect().Size;
		var zoom = Camera.Zoom;

		var size = viewportSize * zoom;
		var topLeft = Camera.GlobalPosition - size / 2f;

		return new Rect2(topLeft, size);
	}

}