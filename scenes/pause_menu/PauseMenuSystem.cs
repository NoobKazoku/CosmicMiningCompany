using CosmicMiningCompany.scripts.events.menu;
using GFramework.Core.extensions;
using GFramework.Core.system;
using global::CosmicMiningCompany.global;
using Godot;

namespace CosmicMiningCompany.scenes.pause_menu;

/// <summary>
/// 暂停菜单系统，负责处理暂停菜单的显示和管理
/// </summary>
public class PauseMenuSystem : AbstractSystem, IPauseMenuSystem
{
    private const string PauseMenuScenePath =
        "res://scenes/pause_menu/pause_menu.tscn";

    private Control? _currentPauseMenu;

    /// <summary>
    /// 初始化暂停菜单系统，注册事件处理器
    /// </summary>
    protected override void OnInit()
    {
        this.RegisterEvent<OpenPauseMenuEvent>(e =>
        {
            // 防止重复打开暂停菜单
            if (_currentPauseMenu != null)
                return;

            var scene = GD.Load<PackedScene>(PauseMenuScenePath);
            _currentPauseMenu = scene.Instantiate<Control>();
            UiRoot.Instance.AddChild(_currentPauseMenu);
        });

        // 注册关闭暂停菜单事件处理器
        this.RegisterEvent<ClosePauseMenuEvent>(_ => { Close(); });
    }

    public void Close()
    {
        if (_currentPauseMenu == null)
            return;

        _currentPauseMenu.QueueFree();
        _currentPauseMenu = null;
    }
}