using Godot;
using System;

public partial class SpaceRock : CharacterBody2D
{
	private Area2D ShootArea => GetNode<Area2D>("%ShootArea");

	public override void _Ready()
    {
        BulletEntered();
    }

	public override void _PhysicsProcess(double delta)
	{
			
	}

    private void BulletEntered()
    {
        ShootArea.BodyEntered += (body) =>
        {
            if (body is Bullet)
            {
                GD.Print("被子弹击中");
            }

        };
    }

    
}
