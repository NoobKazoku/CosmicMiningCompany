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
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
    
    
    /// <summary>
    /// 被陨石调用，销毁子弹
    /// </summary>
    public void DestroyBullet()
    {
        // 停止计时器
        if (_lifeTimer != null)
        {
            _lifeTimer.Stop();
        }
        
        // 销毁子弹
        QueueFree();
    }
}
