using Godot;
using System;

public partial class Loot : CharacterBody2D
{
	public const float Speed = 800.0f;
	public bool IsCollected = false;
	public string OreName = "";

	private SpaceShip SpaceShip => GetTree().Root.GetNode<SpaceShip>("Space/SpaceShip");
	private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("%AnimatedSprite2D");

	private Cargo Cargo => GetTree().Root.GetNode<Cargo>("Space/SpaceShip/Camera2D/UI/Cargo");

    public override void _Ready()
    {

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
		if((SpaceShip.GlobalPosition - GlobalPosition).Length() < 50)
		{
			Collected();
		}
	}

	/// <summary>
	/// 被飞船调用，修改状态为正在被拾取
	/// </summary>
	public void HasCollect()
	{
		IsCollected = true;
	}

	/// <summary>
	/// 播放拾取效果并销毁资源
	/// </summary>
	public void Collected()
	{
		GD.Print("资源已拾取");
		Cargo.CollectOre(OreName);
		QueueFree();
		
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
