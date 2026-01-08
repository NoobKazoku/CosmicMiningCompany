using Godot;
using System;

public partial class SpaceRock : CharacterBody2D
{
	private Area2D ShootArea => GetNode<Area2D>("%ShootArea");

	public override void _Ready()
    {
        SetupCollisionDetection();
    }

	public override void _PhysicsProcess(double delta)
	{
			
	}

    private void SetupCollisionDetection()
    {
        ShootArea.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Bullet bullet)
        {
            GD.Print("被子弹击中");
            // 向子弹发送信号，让它销毁自己
            bullet.DestroyBullet();
        }
    }
}
