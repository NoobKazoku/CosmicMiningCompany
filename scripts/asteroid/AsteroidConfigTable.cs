using CosmicMiningCompany.scripts.enums;
using Godot;
using Godot.Collections;

namespace CosmicMiningCompany.scripts.asteroid;

[GlobalClass]
public partial class AsteroidConfigTable : Resource
{
    [Export] public Array<AsteroidTypeConfig> Configs = null!;

    private System.Collections.Generic.Dictionary<AsteroidType, AsteroidTypeConfig> _cache;

    public void _Ready()
    {
        _cache = new System.Collections.Generic.Dictionary<AsteroidType, AsteroidTypeConfig>();
        foreach (var cfg in Configs)
        {
            _cache[cfg.Type] = cfg;
        }
    }

    public AsteroidTypeConfig Get(AsteroidType type)
    {
        return _cache[type];
    }
}