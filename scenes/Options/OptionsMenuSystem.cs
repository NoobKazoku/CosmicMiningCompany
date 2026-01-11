using CosmicMiningCompany.scripts.events.menu;
using GFramework.Core.extensions;
using GFramework.Core.system;
using Godot;

namespace CosmicMiningCompany.scenes.Options;

/// <summary>
/// 选项菜单系统，负责处理选项菜单的显示和管理
/// </summary>
public class OptionsMenuSystem: AbstractSystem
{
    private const string OptionsScenePath =
        "res://scenes/Options/Options.tscn";
    private Control? _currentOptions;
    
    /// <summary>
    /// 初始化选项菜单系统，注册事件处理器
    /// </summary>
    /// <returns>无返回值</returns>
    protected override void OnInit()
    {
        this.RegisterEvent<OpenOptionsMenuEvent>(e =>
        {
            // 防止重复打开选项菜单
            if (_currentOptions != null)
                return; 
                
            var scene = GD.Load<PackedScene>(OptionsScenePath);
            _currentOptions = scene.Instantiate<Control>();
            e.Node.AddChild(_currentOptions);
        });
        
        // 注册关闭选项菜单事件处理器
        this.RegisterEvent<CloseOptionsMenuEvent>(e =>
        {
            if (_currentOptions == null)
                return;

            _currentOptions.QueueFree();
            _currentOptions = null;
        });
    }
}
