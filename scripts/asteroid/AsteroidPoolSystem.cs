using CosmicMiningCompany.scenes.space_rock;
using CosmicMiningCompany.scripts.core;
using global::CosmicMiningCompany.global;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星对象池系统，用于管理和复用小行星对象以提高性能
/// </summary>
public class AsteroidPoolSystem
    : AbstractNodePoolSystem<string, SpaceRock>,
        IAsteroidPoolSystem
{
    protected override PackedScene LoadScene(string key)
        => AsteroidSceneRegistry.Instance.Get(key);

    protected override void OnInit()
    {
        
    }

    public SpaceRock Acquire(string sceneKey, Node2D parent)
    {
        return Acquire(sceneKey, parent as Node);
    }
}
