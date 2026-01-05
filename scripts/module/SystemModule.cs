using CosmicMiningCompany.scripts.system.impl;
using GFramework.Core.Abstractions.architecture;
using GFramework.Game.Abstractions.assets;
using GFramework.Game.architecture;
using GFramework.Godot.assets;

namespace CosmicMiningCompany.scripts.module;

/// <summary>
/// 系统Godot模块类，负责安装和注册游戏所需的各种系统组件
/// 继承自AbstractGodotModule，用于在游戏架构中集成系统功能
/// </summary>
public class SystemModule: AbstractModule
{
    /// <summary>
    /// 安装方法，用于向游戏架构注册各种系统组件
    /// </summary>
    /// <param name="architecture">游戏架构接口实例，用于注册系统</param>
    public override void Install(IArchitecture architecture)
    {
        // 注册资源目录系统
        architecture.RegisterSystem<IAssetCatalogSystem>(new AssetCatalogSystem());
        // 注册资源加载系统
        architecture.RegisterSystem<IResourceLoadSystem>(new ResourceLoadSystem());
        // 注册资源工厂系统
        architecture.RegisterSystem<IResourceFactorySystem>(new ResourceFactorySystem());
    }
}
