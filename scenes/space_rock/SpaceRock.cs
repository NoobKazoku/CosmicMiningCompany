using CosmicMiningCompany.scripts.asteroid;
using CosmicMiningCompany.scripts.core;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.space_rock;

[Log]
[ContextAware]
public partial class SpaceRock : RigidBody2D, IAsteroid, IController, IPoolableNode
{
    private Timer _lifeTimer;
    private AnimatedSprite2D AnimatedSprite2D => GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
    private Area2D ShootArea => GetNode<Area2D>("%ShootArea");

    // 陨石属性，从JSON加载
    private AsteroidDefinition _definition;

    // 添加标志来确保掉落只执行一次
    private bool _hasDroppedLoot;
    private IAsteroidPoolSystem _pool = null!;
    private int _currentHealth;
    private Timer _recycleTimer;

    public override void _Ready()
    {
        LinearDamp = 0;
        AngularDamp = 0;
        ShootArea.BodyEntered += OnBodyEntered;
        LinearVelocity = new Vector2(GD.RandRange(-50, 50), GD.RandRange(-50, 50));

        // 初始化计时器
        _lifeTimer = new Timer();
        _lifeTimer.WaitTime = 10.0f;
        _lifeTimer.OneShot = true;
        _lifeTimer.Timeout += RequestRecycle;

        AddChild(_lifeTimer);
    }

    public void Init(AsteroidDefinition definition)
    {
        // 初始化AsteroidData
        _definition = definition;
        _currentHealth = definition.BaseHealth;
        // 播放对应的陨石动画
        AnimatedSprite2D.Play(definition.Name);
        _pool = ContextAwareExtensions.GetSystem<IAsteroidPoolSystem>(this)!;
        GD.Print($"陨石初始化: 血量={definition.BaseHealth}, 掉落物={definition.Loot}, 名称={definition.Name}");
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not Bullet bullet) return;
        // 减少血量
        _currentHealth  -= PlayerManager.Instance.Damage; // 每发子弹造成 n点伤害

        if (_currentHealth  <= 0 && !_hasDroppedLoot)
        {
            _hasDroppedLoot = true;
            DropLoot();

            // 播放破碎效果
            AnimatedSprite2D.Visible = false;
            ShootArea.Monitoring = false;
            ShootArea.Monitorable = false;

            var timer = new Timer
            {
                WaitTime = 2.0f,
                OneShot = true
            };
            timer.Timeout += RequestRecycle;
            AddChild(timer);
            timer.Start();
        }


        // 向子弹发送信号，让它销毁自己
        bullet.DestroyBullet();
    }

    private void DropLoot()
    {
        // 实现掉落逻辑
        GD.Print($"小行星掉落: {_definition.Loot}");

        // 生成随机掉落数量 (1-4)
        var dropCount = GD.RandRange(1, 4);

        for (int i = 0; i < dropCount; i++)
        {
            // 加载掉落物场景
            var lootScene = ResourceLoader.Load<PackedScene>("res://scenes/loot/loot.tscn");

            if (lootScene != null)
            {
                // 实例化掉落物
                var lootInstance = (Loot)lootScene.Instantiate();

                // 设置掉落物的位置（稍微分散一些）
                var offset = new Vector2((float)GD.RandRange(-20, 21), (float)GD.RandRange(-20, 21));
                lootInstance.GlobalPosition = this.GlobalPosition + offset;

                lootInstance.Initialize(_definition.Loot);

                // 添加到场景树（使用CallDeferred避免在物理回调中修改场景树）
                GetParent().CallDeferred("add_child", lootInstance);

                GD.Print($"生成掉落物: {_definition.Loot} #{i + 1}");
            }
            else
            {
                GD.PrintErr("无法加载掉落物场景: res://scenes/loot/loot.tscn");
            }
        }
    }

    public void ScheduleDestroy(float delay)
    {
        var timer = new Timer
        {
            WaitTime = delay,
            OneShot = true
        };
        timer.Timeout += RequestRecycle;
        AddChild(timer);
        timer.Start();
    }
    
    public void OnAcquire()
    {
        _hasDroppedLoot = false;

        LinearVelocity = Vector2.Zero;
        AngularVelocity = 0;
        AnimatedSprite2D.Visible = true;
        ShootArea.Monitoring = true;
        ShootArea.Monitorable = true;
        var collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        collisionShape?.Disabled = false;

        _lifeTimer.Start();
    }


    public void OnRelease()
    {
        _lifeTimer.Stop();

        GetNodeOrNull<CpuParticles2D>("break/CPUParticles2D_break")?.Emitting = false;
        GetNodeOrNull<CpuParticles2D>("break/CPUParticles2D_blast")?.Emitting = false;
    }

    public void OnPoolDestroy()
    {
        ShootArea.BodyEntered -= OnBodyEntered;
        _lifeTimer.QueueFree();
    }


    public void RequestRecycle()
    {
        _pool.Release(_definition.SceneKey, this);
    }
}