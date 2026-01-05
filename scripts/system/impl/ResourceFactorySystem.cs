using GFramework.Godot.assets;
using GFramework.SourceGenerators.Abstractions.logging;

namespace CosmicMiningCompany.scripts.system.impl;

/// <summary>
/// 资源工厂系统，负责管理和创建各种游戏资源的工厂实例。
/// 该系统通过注册表管理不同类型的资源工厂，并支持场景和资源的预加载功能。
/// </summary>
[Log]
public partial class ResourceFactorySystem : AbstractResourceFactorySystem
{
    /// <summary>
    /// 注册系统所需的各种资源类型。
    /// </summary>
    protected override void RegisterResources()
    {
        _log.Debug("开始注册资源");
        // RegisterSceneUnit<Unit>(AssetCatalogConstants.AssetCatalogSceneUnit.Unit.Key, true);
        // RegisterAsset<UnitDataResource>(AssetCatalogConstants.AssetCatalogResource.Robin.Key,true);
    }
}