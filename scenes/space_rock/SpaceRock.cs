using Godot;
using System;

public partial class SpaceRock : RigidBody2D
{
    private Timer _lifeTimer;
	private Area2D ShootArea => GetNode<Area2D>("%ShootArea");
    private SpaceShip SpaceShip => GetTree().Root.GetNode<SpaceShip>("Space/SpaceShip");

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
            // 向子弹发送信号，让它销毁自己
            bullet.DestroyBullet();
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