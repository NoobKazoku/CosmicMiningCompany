using GFramework.Core.system;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星生成系统，负责根据配置规则在指定位置生成小行星
/// </summary>
/// <param name="spawnConfig">生成配置参数</param>
/// <param name="configTable">小行星配置表</param>
/// <param name="worldRoot">世界根节点，用于添加生成的小行星</param>
public class AsteroidSpawnSystem(
    AsteroidSpawnConfig spawnConfig,
    AsteroidConfigTable configTable,
    Node worldRoot): AbstractSystem
{
    private readonly AsteroidSpawnRule _rule = new(spawnConfig);
    private readonly AsteroidFactory _factory = new(configTable);

    /// <summary>
    /// 尝试在指定位置生成小行星
    /// </summary>
    /// <param name="spawnPosition">生成位置</param>
    public void TrySpawn(Vector2 spawnPosition)
    {
        var distance = spawnPosition.Length();

        // 检查是否满足生成条件
        if (!_rule.CanSpawn(distance))
            return;

        // 根据距离决定小行星类型
        var type = _rule.Decide(distance);
        var asteroid = _factory.Create(type, spawnPosition);

        // 将生成的小行星添加到世界中
        worldRoot.AddChild(asteroid);
    }

    protected override void OnInit()
    {
        
    }
}

// using Godot;
//
// public partial class AsteroidSpawner : Node2D
// {
//     [Export] public AsteroidSpawnConfig SpawnConfig;
//     [Export] public AsteroidConfigTable ConfigTable;
//
//     private AsteroidSpawnSystem _spawnSystem;
//
//     public override void _Ready()
//     {
//         _spawnSystem = new AsteroidSpawnSystem(
//             SpawnConfig,
//             ConfigTable,
//             GetTree().CurrentScene
//         );
//     }
//
//     public override void _Process(double delta)
//     {
//         if (GD.Randf() < 0.02f)
//         {
//             Vector2 pos = RandomPosition();
//             _spawnSystem.TrySpawn(pos);
//         }
//     }
//
//     private Vector2 RandomPosition()
//     {
//         float angle = GD.Randf() * Mathf.Tau;
//         float distance = GD.RandRange(500, 4000);
//         return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
//     }
// }

