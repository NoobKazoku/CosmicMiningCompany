using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;

[ContextAware]
[Log]
public partial class SpaceShip :CharacterBody2D,IController
{
    [Export] public float RotationSpeed = 5.0f; //转向角速度
    
    // 移动相关参数
    [Export] public float Acceleration = 200;      // 推力加速度
    [Export] public float MaxSpeed = 300;          // 最大速度
    [Export] public float BrakeForce = 150;          // 制动力
    private bool isMoving = false;                    // 是否正在移动
    

    //飞船属性
    [Export] public float MaxFuel = 100.0f;
    [Export] public float Fuel  = 100.0f;
    [Export] public float FuelConsumptionRate = 0.333f; // 每秒消耗的燃料量 (100燃料/300秒 = 0.333)

    public Gun Gun => GetNode<Gun>("%Gun");
    private Area2D LootArea => GetNode<Area2D>("%LootArea");
	
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
        //初始化全局变量
        RotationSpeed = PlayerManager.Instance.RotationSpeed; //转向角速度
        Acceleration = PlayerManager.Instance.Acceleration;      // 推力加速度
        MaxSpeed = PlayerManager.Instance.Speed;          // 最大速度
        BrakeForce = PlayerManager.Instance.BrakeForce;          // 制动力
        MaxFuel = PlayerManager.Instance.MaxFuel;
        Fuel = PlayerManager.Instance.MaxFuel;
        FuelConsumptionRate = PlayerManager.Instance.FuelConsumptionRate; // 每秒消耗的燃料量 (100燃料/300秒 = 0.333)

        LootArea.Scale = PlayerManager.Instance.PickupRange * Vector2.One;



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
        
        // 应用推力（加速度）
        if (inputDirection.Length() > 0)
        {
            Vector2 thrust = inputDirection * Acceleration;
            
            // 计算新速度
            Vector2 newVelocity = Velocity + thrust * delta;
            
            // 如果新速度没有超过最大速度，直接应用
            if (newVelocity.Length() <= MaxSpeed)
            {
                Velocity = newVelocity;
            }
            else
            {
                // 如果新速度超过了最大速度
                // 有两种情况需要处理：
                // 1. 如果输入方向与当前速度方向相似（前进加速），则限制速度
                // 2. 如果输入方向与当前速度方向不同（转向），则允许改变方向
                
                float dotProduct = Velocity.Normalized().Dot(inputDirection);
                
                if (dotProduct > 0.5f) // 大致向前（>90度夹角）
                {
                    // 限制速度到最大值
                    Velocity = newVelocity.Normalized() * MaxSpeed;
                }
                else // 转向或后退
                {
                    // 让玩家能够转向，逐渐改变方向
                    // 计算一个平衡当前速度和期望速度的混合速度
                    Vector2 desiredVelocity = inputDirection * MaxSpeed;
                    
                    // 使用插值来平滑转向
                    float turnRate = 0.1f; // 可以根据需要调整转向率
                    Vector2 mixedVelocity = Velocity.Lerp(desiredVelocity, turnRate);
                    
                    // 确保最终速度不超过最大速度
                    if (mixedVelocity.Length() > MaxSpeed)
                    {
                        mixedVelocity = mixedVelocity.Normalized() * MaxSpeed;
                    }
                    
                    Velocity = mixedVelocity;
                }
            }
        }
        else
        {
            // 没有输入时应用制动
            if (Velocity.Length() > 0.1f)
            {
                // 计算制动方向（与当前速度相反）
                Vector2 brakeDirection = -Velocity.Normalized();
                
                // 应用制动力
                Vector2 brakeForceVector = brakeDirection * BrakeForce;
                
                // 更新速度（制动）
                Vector2 newVelocity = Velocity + brakeForceVector * delta;
                
                // 防止过度制动导致反向运动
                if (newVelocity.Length() > Velocity.Length())
                {
                    // 如果制动后速度反而增加（即已经是负向），则停止
                    Velocity = Vector2.Zero;
                }
                else
                {
                    Velocity = newVelocity;
                }
            }
            else
            {
                Velocity = Vector2.Zero;
            }
        }
        
        // 确保最终速度不超过最大速度
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
            if (Fuel <= 0)
            {
                Fuel = 0;
                GD.Print("燃料耗尽！");
                //回到空间站
                GetTree().ChangeSceneToFile("res://scenes/space_station/space_station.tscn");
            }
        }
    }

    public void OnLootEntered(Node body)
    {
        if (body is Loot loot)
        {
            // 向资源发送信号，让它销毁自己
            loot.HasCollect();
        }
    }
}