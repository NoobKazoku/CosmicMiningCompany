using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;

[ContextAware]
[Log]
public partial class SpaceShip :CharacterBody2D,IController
{
    // 旋转速度
    [Export] public float RotationSpeed = 5.0f;
    
    // 移动相关参数
    [Export] public float Acceleration = 400.0f;      // 推力加速度
    [Export] public float MaxSpeed = 600.0f;          // 最大速度
    [Export] public float BrakeForce = 300.0f;          // 制动力
    private bool isMoving = false;                    // 是否正在移动
    

    //飞船属性
    [Export] public float Fuel  = 100.0f;
    [Export] public float MaxFuel = 100.0f;
    [Export] public float FuelConsumptionRate = 0.333f; // 每秒消耗的燃料量 (100燃料/300秒 = 0.333)
    
    public Gun Gun => GetNode<Gun>("%Gun");
    private Area2D LootArea => GetNode<Area2D>("%LootArea");
	
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		LootArea.BodyEntered += OnLootEntered;
	}

	public override void _PhysicsProcess(double delta)
    {
        // 处理移动
        HandleMovement((float)delta);
        
        // 处理旋转
        RotateToMouse(delta);
        
        // 处理燃料消耗
        if (isMoving)
        {
            ConsumeFuel((float)delta);
        }
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
            
            // 平滑旋转
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
        isMoving = false; // 重置移动状态
        
        // 检测方向键输入
        if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up))
        {
            inputDirection.Y -= 1; // 向上移动
            isMoving = true;
        }
        if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
        {
            inputDirection.Y += 1; // 向下移动
            isMoving = true;
        }
        if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
        {
            inputDirection.X -= 1; // 向左移动
            isMoving = true;
        }
        if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
        {
            inputDirection.X += 1; // 向右移动
            isMoving = true;
        }
        
        // 标准化输入方向向量，确保对角线移动速度不会过快
        if (inputDirection.Length() > 0)
        {
            inputDirection = inputDirection.Normalized();
        }
        
        // 分离处理推力和制动
        
        // 只应用推力
        if (inputDirection.Length() > 0)
        {
            // 应用推力（加速度）
            Vector2 thrust = inputDirection * Acceleration;
            
            // 计算应用推力后的新速度
            Vector2 newVelocity = Velocity + thrust * delta;
            
            // 如果新速度超过最大速度，则限制推力大小
            if (newVelocity.Length() > MaxSpeed)
            {
                // 计算允许的最大速度变化
                float maxSpeedChange = Mathf.Max(0, MaxSpeed - Velocity.Length());
                if (maxSpeedChange > 0)
                {
                    // 限制推力大小，使速度不超过最大速度
                    thrust = thrust.Normalized() * (maxSpeedChange / delta);
                    Velocity += thrust * delta;
                }
                // 如果已经达到或超过最大速度，不应用推力
            }
            else
            {
                // 新速度不超过最大速度，正常应用推力
                Velocity = newVelocity;
            }
        }
        
        // 制动处理
        bool shouldApplyBrake = false;
        
        if (inputDirection.Length() == 0 && Velocity.Length() > 0.1f)
        {
            // 没有输入时应用制动
            shouldApplyBrake = true;
        }
        else if (inputDirection.Length() > 0 && Velocity.Length() > 0.1f)
        {
            // 有输入时，检查输入方向是否与当前移动方向相反
            Vector2 normalizedVelocity = Velocity.Normalized();
            float dotProduct = inputDirection.Dot(normalizedVelocity);
            if (dotProduct < -0.1f) // 方向几乎相反（允许小的角度误差）
            {
                shouldApplyBrake = true;
            }
        }
        
        // 应用制动
        if (shouldApplyBrake)
        {
            // 计算制动方向（与当前速度相反）
            Vector2 brakeDirection = -Velocity.Normalized();
            
            // 应用制动力
            Vector2 brakeForceVector = brakeDirection * BrakeForce;
            
            // 更新速度（制动）
            Velocity += brakeForceVector * delta;
            
            // 防止过度制动导致反向运动
            if (Velocity.Length() > 0 && brakeDirection.Dot(Velocity.Normalized()) > 0)
            {
                Velocity = Vector2.Zero;
            }
        }
        
        // 在所有速度更新完成后统一限制最大速度
        if (Velocity.Length() > MaxSpeed)
        {
            Velocity = Velocity.Normalized() * MaxSpeed;
        }
        
        // 使用CharacterBody2D的MoveAndSlide方法移动飞船
        MoveAndSlide();
    }
    
    /// <summary>
    /// 消耗燃料
    /// </summary>
    /// <param name="delta">时间步长</param>
    private void ConsumeFuel(float delta)
    {
        if (Fuel > 0)
        {
            Fuel -= FuelConsumptionRate * delta;
            if (Fuel < 0)
            {
                Fuel = 0;
                GD.Print("燃料耗尽！");
            }
        }
    }

    public void OnLootEntered(Node body)
    {
        if (body is Loot loot)
        {
            GD.Print("检测到到资源");
            // 向资源发送信号，让它销毁自己
            loot.HasCollect();
        }
    }
}
