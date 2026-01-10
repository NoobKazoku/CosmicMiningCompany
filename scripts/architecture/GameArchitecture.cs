using CosmicMiningCompany.scripts.module;
using GFramework.Core.Abstractions.architecture;
using GFramework.Core.Abstractions.environment;
using GFramework.Godot.architecture;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;

namespace CosmicMiningCompany.scripts.architecture;

/// <summary>
/// 游戏架构类，负责安装和管理游戏所需的各种模块
/// 继承自AbstractArchitecture，用于构建游戏的整体架构体系
/// </summary>
/// <param name="configuration">架构配置对象，包含日志记录等配置信息</param>
/// <param name="environment">游戏环境对象，用于提供环境相关的服务</param>
[Log]
[ContextAware]
public sealed partial class GameArchitecture(IArchitectureConfiguration configuration, IEnvironment environment) : AbstractArchitecture(configuration, environment)
{
    /// <summary>
    /// 获取或设置架构配置对象
    /// </summary>
    public IArchitectureConfiguration Configuration { get; set; } = configuration;

    /// <summary>
    /// 安装游戏所需的各个功能模块
    /// 该方法在架构初始化时被调用，用于注册系统、模型和工具模块
    /// </summary>
    protected override void InstallModules()
    {
        // 安装数据模型相关的Godot模块
        InstallModule(new ModelModule());
        // 安装系统相关的Godot模块
        InstallModule(new SystemModule());
        // 安装工具类相关的Godot模块
        InstallModule(new UtilityModule());
        
        _log.Debug("所有模块安装完成");
    }
}