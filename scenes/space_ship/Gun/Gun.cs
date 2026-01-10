using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Gun : Sprite2D, IController
{
	[Export] public float FireRate = 0.2f; // 射击间隔，单位秒
	private float _timeSinceLastShot = 0f;
	private PackedScene _bulletScene;

	// 过热机制相关变量
	private float _heat = 0f; // 当前过热值
	private float MAX_HEAT = 100; // 最大过热值
	private bool _isOverheated = false; // 是否过热
	private float COOL_DOWN_RATE_NORMAL = 5; // 正常冷却速率：每秒降低5点
	private float COOL_DOWN_RATE_OVERHEAT = 20; // 过热冷却速率：每秒降低20点

	// 公共属性，供UI访问
	public float Heat => _heat;
	public float MaxHeat => MAX_HEAT;
	public bool IsOverheated => _isOverheated;

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		//初始化全局变量
		FireRate = PlayerManager.Instance.FireRate; // 射击间隔，单位秒
		MAX_HEAT = PlayerManager.Instance.MaxHeat; // 最大过热值
		COOL_DOWN_RATE_NORMAL = PlayerManager.Instance.ColdDownRateNormal; // 正常冷却速率：每秒降低5点
		COOL_DOWN_RATE_OVERHEAT = PlayerManager.Instance.ColdDownRateOverHeat; // 过热冷却速率：每秒降低20点

		_bulletScene = GD.Load<PackedScene>("res://scenes/space_ship/Bullet/Bullet.tscn");
	}

	public override void _PhysicsProcess(double delta)
	{
		_timeSinceLastShot += (float)delta;
		var particlesNode = GetNodeOrNull<CpuParticles2D>("CPUParticles2D_shell casing");
		if (particlesNode != null)
		{
			particlesNode.AngleMin = -GlobalRotationDegrees;
			particlesNode.AngleMax = -GlobalRotationDegrees;
		}

		// 处理冷却
		HandleCooling((float)delta);

		// 检查是否按下射击键
		if (Input.IsActionPressed("shoot") && _timeSinceLastShot >= FireRate && !_isOverheated)
		{
			Shoot();
			_timeSinceLastShot = 0f;
			if (particlesNode != null)
			{
				particlesNode.Emitting = true; // 启动粒子效果
			}
		}
		else if (!Input.IsActionPressed("shoot"))
		{
			// 检测到松开射击键时，停止弹壳粒子效果
			if (particlesNode != null)
			{
				particlesNode.Emitting = false; // 停止粒子效果
			}
		}
	}

	private void Shoot()
	{
		// 检查是否过热
		if (_isOverheated)
			return;

		// 实例化子弹
		var bullet = _bulletScene.Instantiate<Bullet>();

		// 计算子弹在枪前方10像素的位置
		// 获取相对于枪的偏移（向枪的x轴正方向偏移10像素）
		var localOffset = new Vector2(10, 0); // 相对枪的局部偏移

		// 将局部偏移转换为全局偏移，考虑枪的旋转
		var globalOffset = localOffset.Rotated(GlobalRotation);

		// 计算子弹的全局位置
		bullet.GlobalPosition = GlobalPosition + globalOffset;

		// 设置子弹方向为枪口方向
		bullet.Rotation = GlobalRotation;

		// 将子弹添加到场景根节点，确保子弹不受飞船移动和旋转的影响
		// 使用GetTree().Root.AddChild()将子弹添加到场景树根节点
		GetTree().Root.AddChild(bullet);

		// 给子弹一个向前的速度（基于发射时的全局方向）
		// 这里我们使用枪的全局旋转来确定初始发射方向
		var initialDirection = Vector2.Right.Rotated(GlobalRotation);
		bullet.Velocity = initialDirection * bullet.BulletSpeed;

		// 增加过热值
		_heat += PlayerManager.Instance.ColdUpRateNormal;

		// 检查是否过热
		if (_heat >= MAX_HEAT)
		{
			_heat = MAX_HEAT;
			_isOverheated = true;
		}



	}

	/// <summary>
	/// 处理冷却逻辑
	/// </summary>
	/// <param name="delta">时间步长</param>
	private void HandleCooling(float delta)
	{
		if (_isOverheated)
		{
			// 过热冷却：无论是否按下射击键，每秒降低20点
			_heat -= COOL_DOWN_RATE_OVERHEAT * delta;
			if (_heat <= 0)
			{
				_heat = 0;
				_isOverheated = false;
			}
		}
		else
		{
			// 正常冷却：只有在没有按下射击键时才冷却，每秒降低5点
			if (!Input.IsActionPressed("shoot") && _heat > 0)
			{
				_heat -= COOL_DOWN_RATE_NORMAL * delta;
				if (_heat < 0)
					_heat = 0;
			}
		}
	}
}
