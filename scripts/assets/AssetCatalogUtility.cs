using CosmicMiningCompany.scripts.constants;
using GFramework.Game.assets;
using GFramework.SourceGenerators.Abstractions.logging;

namespace CosmicMiningCompany.scripts.assets;

/// <summary>
/// 资源目录系统类，用于管理和注册游戏中的各种资源
/// </summary>
[Log]
public partial class AssetCatalogUtility : AbstractAssetCatalogUtility
{
    /// <summary>
    /// 注册游戏所需的所有资源
    /// 该方法在系统初始化时被调用，用于将资源键与资源路径进行映射关联
    /// </summary>
    protected override void RegisterAssets()
    {
        _log.Debug("开始注册资产");
        
        // 注册游戏中的核心资源
        // 武器资源
        RegisterAsset(AssetCatalogConstants.AssetCatalogResource.BulletScene, "res://scenes/space_ship/Bullet/Bullet.tscn");
        RegisterAsset(AssetCatalogConstants.AssetCatalogResource.GunScene, "res://scenes/space_ship/Gun/Gun.tscn");
        
        // 小行星资源
        RegisterAsset(AssetCatalogConstants.AssetCatalogResource.SpaceRockScene, "res://scenes/space_rock/space_rock.tscn");
        
        // 战利品资源
        RegisterAsset(AssetCatalogConstants.AssetCatalogResource.LootScene, "res://scenes/loot/loot.tscn");
        
        // 场景资源
        RegisterAsset(AssetCatalogConstants.AssetCatalogResource.SpaceScene, "res://scenes/space/space.tscn");
        RegisterAsset(AssetCatalogConstants.AssetCatalogResource.SpaceStationScene, "res://scenes/space_station/space_station.tscn");
        
        _log.Debug("资产注册完成");
    }
}