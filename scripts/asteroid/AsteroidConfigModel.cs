using System.Collections.Generic;
using CosmicMiningCompany.scripts.constants;
using CosmicMiningCompany.scripts.enums;
using GFramework.Core.extensions;
using GFramework.Core.model;
using GFramework.Game.Abstractions.assets;

namespace CosmicMiningCompany.scripts.asteroid;

public class AsteroidConfigModel: AbstractModel
{
    private AsteroidConfigTable _asteroidConfigTable = null!;
    private readonly Dictionary<AsteroidType, AsteroidTypeConfig> _asteroidTypeConfigTableDic = new();
    public void Initialize()
    { 
        var resourceFactorySystem = this.GetSystem<IResourceFactorySystem>();
        _asteroidConfigTable = resourceFactorySystem!.GetFactory<AsteroidConfigTable>(AssetCatalogConstants.AssetCatalogResource.AsteroidConfigTable.Key).Invoke();
        foreach (var cfg in _asteroidConfigTable.Configs)
        {
            _asteroidTypeConfigTableDic[cfg.Type] = cfg;
        }
    }
    public AsteroidTypeConfig Get(AsteroidType type)
    {
        return _asteroidTypeConfigTableDic[type];
    }
    protected override void OnInit()
    {
        
    }
    
}