using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Gun :Sprite2D,IController
{
    [Export] public float FireRate = 0.2f; // 射击间隔，单位秒
    private float _timeSinceLastShot = 0f;
    private PackedScene _bulletScene;
    
    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _bulletScene = GD.Load<PackedScene>("res://scenes/space_ship/Bullet/Bullet.tscn");
    }

    public override void _PhysicsProcess(double delta)
    {
        _timeSinceLastShot += (float)delta;
        
        // 检查是否按下射击键
        if (Input.IsActionPressed("shoot") && _timeSinceLastShot >= FireRate)
        {
            Shoot();
            _timeSinceLastShot = 0f;
        }
    }

    private void Shoot()
    {
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
    }
}
