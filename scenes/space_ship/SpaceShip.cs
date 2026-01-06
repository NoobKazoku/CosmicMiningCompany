using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class SpaceShip :Node2D,IController
{
    // 添加一个字段来控制旋转速度（可选）
    [Export] public float RotationSpeed = 5.0f;
    
    // 移动相关参数
    [Export] public float Acceleration = 20.0f;      // 加速度
    [Export] public float MaxSpeed = 150.0f;          // 最大速度
    [Export] public float Friction = 0.95f;           // 摩擦力（接近1表示摩擦力小）
    
    // 存储飞船的速度
    private Vector2 velocity = Vector2.Zero;

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
    {
        // 处理移动
        HandleMovement((float)delta);
        
        // 处理旋转
        RotateToMouse(delta);
        
    }

	public void RotateToMouse(double delta)
	{
        // 获取鼠标在场景中的全局位置
        Vector2 mousePosition = GetGlobalMousePosition();
        
        // 获取当前节点的全局位置
        Vector2 shipPosition = GlobalPosition;
        
        // 计算从飞船到鼠标位置的方向向量
        Vector2 direction = mousePosition - shipPosition;
        
        // 如果方向向量不是零向量，则更新飞船的旋转角度
        if (direction.Length() > 0)
        {
            // 计算目标旋转角度
            float targetRotation = direction.Angle();
            
            // 平滑旋转（可选，如果想要平滑效果）
            Rotation = Mathf.LerpAngle(Rotation, targetRotation, (float)delta * RotationSpeed);
            
        }
	}
	
	/// <summary>
    /// 处理飞船移动
    /// </summary>
    /// <param name="delta">时间步长</param>
    private void HandleMovement(float delta)
    {
        Vector2 inputDirection = Vector2.Zero;
        
        // 检测方向键输入
        if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up))
        {
            inputDirection.Y -= 1; // 向上移动
        }
        if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
        {
            inputDirection.Y += 1; // 向下移动
        }
        if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
        {
            inputDirection.X -= 1; // 向左移动
        }
        if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
        {
            inputDirection.X += 1; // 向右移动
        }
        
        // 标准化输入方向向量，确保对角线移动速度不会过快
        if (inputDirection.Length() > 0)
        {
            inputDirection = inputDirection.Normalized();
        }
        
        // 根据输入方向更新速度
        if (inputDirection.Length() > 0)
        {
            // 应用加速度
            velocity += inputDirection * Acceleration * delta;
            
            // 限制最大速度
            if (velocity.Length() > MaxSpeed)
            {
                velocity = velocity.Normalized() * MaxSpeed;
            }
        }
        else
        {
            // 应用摩擦力 - 当没有输入时逐渐减慢速度
            velocity *= Friction;
            
            // 如果速度变得非常小，将其设置为零以避免无限接近零的小值
            if (velocity.Length() < 1.0f)
            {
                velocity = Vector2.Zero;
            }
        }
        
        // 根据速度更新飞船位置
        Position += velocity;
    }
}