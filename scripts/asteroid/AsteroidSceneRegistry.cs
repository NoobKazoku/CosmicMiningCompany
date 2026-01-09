using Godot;
using Godot.Collections;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星场景注册表，继承自Node，实现IAsteroidSceneRegistry接口，管理小行星场景资源
/// </summary>
public partial class AsteroidSceneRegistry : Node, IAsteroidSceneRegistry
{
    public static AsteroidSceneRegistry Instance { get;private set; } = null!;
    /// <summary>
    /// 导出的场景字典，存储小行星场景键值对
    /// </summary>
    [Export]
    public Dictionary<string, PackedScene> Scenes { get; set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// 根据指定的键获取打包的场景
    /// </summary>
    /// <param name="key">用于标识场景的键值</param>
    /// <returns>与键关联的打包场景对象</returns>
    public PackedScene Get(string key)
    {
        return Scenes[key];
    }
}