using Godot;

namespace CosmicMiningCompany.global;

/// <summary>
/// 开发绑定管理器，用于管理开发模式下的数据绑定和资源引用
/// 继承自Godot Node类，提供场景节点功能
/// </summary>
public partial class DevBindingManager : Node
{
    /// <summary>
    /// DevBindingManager 单例实例
    /// </summary>
    public static DevBindingManager Instance { get; private set; } = null!;
    
    public override void _Ready()
    {
        Instance = this;
    }
}
