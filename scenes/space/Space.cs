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

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		_asteroidSpawnSystem = this.GetSystem<IAsteroidSpawnSystem>()!;
		_spawnTimer = new Timer
		{
			WaitTime = 1.0f,
			OneShot = false,
			Autostart = true
		};
		_spawnTimer.Timeout += OnSpawnTimeout;
		AddChild(_spawnTimer);
	}
	private void OnSpawnTimeout()
	{
		var spawnPosition = RandomSpawnPosition();
		_asteroidSpawnSystem.TrySpawn(this, spawnPosition);
	}
	private Vector2 RandomSpawnPosition()
	{
		var angle = GD.Randf() * Mathf.Tau;
		float distance = GD.RandRange(600, 3000);
		return new Vector2(
			Mathf.Cos(angle),
			Mathf.Sin(angle)
		) * distance;
	}
}