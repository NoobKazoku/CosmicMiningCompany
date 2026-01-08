using GFramework.Game.Abstractions.assets;

namespace CosmicMiningCompany.scripts.constants;

/// <summary>
/// 资产目录常量类，用于定义游戏中各种资产的配置映射
/// </summary>
public class AssetCatalogConstants
{
    /// <summary>
    /// 资产目录资源配置类，包含游戏中各种资源文件的映射关系
    /// </summary>
    public static class AssetCatalogResource
    {
        /// <summary>
        /// 小行星配置文件的资产映射
        /// </summary>
        public static readonly AssetCatalog.AssetCatalogMapping AsteroidConfigTable = new("AsteroidConfigTable",new AssetCatalog.AssetId("res://resource/asteroid/asteroid_config_table.tres"));
    }
}
