using CosmicMiningCompany.scripts.architecture;
using CosmicMiningCompany.scripts.asteroid;
using CosmicMiningCompany.scripts.command.setting;
using CosmicMiningCompany.scripts.data.interfaces;
using CosmicMiningCompany.scripts.environment;
using CosmicMiningCompany.scripts.query;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.Abstractions.logging;
using GFramework.Core.Abstractions.properties;
using GFramework.Core.architecture;
using GFramework.Core.extensions;
using GFramework.Core.query;
using GFramework.Game.Abstractions.assets;
using GFramework.Godot.logging;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.global;

/// <summary>
/// 游戏入口点节点，负责初始化和清理游戏架构
/// </summary>
[Log]
[ContextAware]
public partial class GameEntryPoint : Node
{
    private GameArchitecture _architecture = null!;
    private ISaveStorageUtility _saveStorageUtility = null!;
    private ISettingsStorageUtility _settingsStorageUtility = null!;
    private ISettingsModel _settingsModel = null!;
    public static GameEntryPoint Instance { get; private set; } = null!;
    /// <summary>
    /// 当节点准备好时调用，初始化游戏架构
    /// </summary>
    /// <returns>无返回值</returns>
    public override void _Ready()
    {
        // 创建并初始化游戏架构实例
        // 配置架构的日志记录属性，设置Godot日志工厂提供程序并指定最低日志级别为调试级别
        // 然后初始化架构实例以准备游戏运行环境
        _architecture = new GameArchitecture(new ArchitectureConfiguration
        {
            LoggerProperties = new LoggerProperties()
            {
                LoggerFactoryProvider = new GodotLoggerFactoryProvider
                {
                    MinLevel = LogLevel.Debug
                }
            }
        }, new GameDevEnvironment());
        _architecture.Initialize();
        _saveStorageUtility = this.GetUtility<ISaveStorageUtility>()!;
        _settingsStorageUtility = this.GetUtility<ISettingsStorageUtility>()!;
        _settingsModel = this.GetModel<ISettingsModel>()!;
        Instance= this;
        var data = _settingsStorageUtility.Load();
        this.SendCommand(new ApplySettingsDataCommand(new ApplySettingsDataCommandInput
        {
            Settings = data
        }));
        _log.Info("设置已加载");
    }

    /// <summary>
    /// 当节点从场景树中移除时调用，销毁游戏架构
    /// </summary>
    /// <returns>无返回值</returns>
    public override void _ExitTree()
    {
        _saveStorageUtility.Save();
        _settingsStorageUtility.Save(_settingsModel.GetSettingsData());
        // 安全销毁游戏架构实例
        _architecture.Destroy();
    }
}