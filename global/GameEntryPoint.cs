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
/// 作为游戏的核心入口，管理游戏的启动、运行和关闭过程
/// </summary>
[Log]
[ContextAware]
public partial class GameEntryPoint : Node
{
    /// <summary>
    /// 游戏架构实例，负责管理游戏的整体架构
    /// </summary>
    private GameArchitecture _architecture = null!;
    
    /// <summary>
    /// 存档存储工具，用于保存游戏进度
    /// </summary>
    private ISaveStorageUtility _saveStorageUtility = null!;
    
    /// <summary>
    /// 设置存储工具，用于保存和加载游戏设置
    /// </summary>
    private ISettingsStorageUtility _settingsStorageUtility = null!;
    
    /// <summary>
    /// 设置模型，用于管理游戏的设置数据
    /// </summary>
    private ISettingsModel _settingsModel = null!;
    
    /// <summary>
    /// 游戏入口点的单例实例，提供全局访问点
    /// </summary>
    public static GameEntryPoint Instance { get; private set; } = null!;
    
    /// <summary>
    /// 当节点准备好时调用，初始化游戏架构
    /// 这是游戏启动时的第一个关键方法，负责构建游戏的运行环境
    /// </summary>
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
        
        // 初始化游戏架构，加载所有模块和服务
        _architecture.Initialize();
        
        // 获取必要的工具和模型实例
        _saveStorageUtility = this.GetUtility<ISaveStorageUtility>()!;
        _settingsStorageUtility = this.GetUtility<ISettingsStorageUtility>()!;
        _settingsModel = this.GetModel<ISettingsModel>()!;
        
        // 设置单例实例
        Instance = this;
        
        // 加载并应用游戏设置
        var data = _settingsStorageUtility.Load();
        this.SendCommand(new ApplySettingsDataCommand(new ApplySettingsDataCommandInput
        {
            Settings = data
        }));
        
        _log.Info("设置已加载");
    }

    /// <summary>
    /// 当节点从场景树中移除时调用，销毁游戏架构
    /// 这是游戏关闭时的关键方法，负责清理资源和保存数据
    /// </summary>
    public override void _ExitTree()
    {
        // 保存游戏进度和设置
        _saveStorageUtility.Save();
        _settingsStorageUtility.Save(_settingsModel.GetSettingsData());
        
        // 安全销毁游戏架构实例，释放所有资源
        _architecture.Destroy();
    }
}