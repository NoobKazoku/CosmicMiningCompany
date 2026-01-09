using CosmicMiningCompany.scripts.assets;
using CosmicMiningCompany.scripts.data;
using CosmicMiningCompany.scripts.resources;
using CosmicMiningCompany.scripts.setting;
using CosmicMiningCompany.scripts.storage;
using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;
using GFramework.Godot.assets;

namespace CosmicMiningCompany.scripts.module;



/// <summary>
/// 工具模块类，负责安装和管理游戏中的实用工具组件
/// </summary>
public class UtilityModule: AbstractModule
{
    /// <summary>
    /// 安装模块到指定的游戏架构中
    /// </summary>
    /// <param name="architecture">要安装模块的目标游戏架构实例</param>
    public override void Install(IArchitecture architecture)
    {
        architecture.RegisterUtility(new FileSaveStorage());
        architecture.RegisterUtility(new SaveStorageUtility());
        architecture.RegisterUtility(new SettingsStorageUtility());
        // 注册资源目录系统
        architecture.RegisterUtility(new AssetCatalogUtility());
        // 注册资源加载系统
        architecture.RegisterUtility(new ResourceLoadUtility());
        // 注册资源工厂系统
        architecture.RegisterUtility(new ResourceFactoryUtility());
    }
}


