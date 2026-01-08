using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    [Export]public float BulletSpeed = 800.0f;
    private Timer _lifeTimer;
	
    public override void _Ready()
    {
        // 创建并启动计时器，3秒后销毁子弹
        _lifeTimer = new Timer();
        _lifeTimer.WaitTime = 3.0f;
        _lifeTimer.Timeout += () => QueueFree(); // 3秒后销毁子弹
        _lifeTimer.OneShot = true;
        AddChild(_lifeTimer);
        _lifeTimer.Start();
        
        GetNode<Area2D>("%BulletArea").AreaEntered += (area) =>
        {
            // 当子弹进入Area2D时，销毁子弹
            GD.Print("子弹命中");
            _lifeTimer = new Timer();
            _lifeTimer.WaitTime = 0.01f;
            _lifeTimer.Timeout += () => QueueFree();
            AddChild(_lifeTimer);
            _lifeTimer.Start();
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
    
    /// <summary>
    /// 当子弹进入Area2D时调用（例如与ShootArea碰撞）
    /// </summary>
    /// <param name="area">进入的Area2D节点</param>

}
