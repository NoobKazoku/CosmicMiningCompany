using CosmicMiningCompany.scripts.enums;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

public class AsteroidFactory(AsteroidConfigTable configTable)
{
    public Node2D Create(AsteroidType type, Vector2 position)
    {
        var cfg = configTable.Get(type);
        var node = cfg.Scene.Instantiate<Node2D>();

        node.Position = position;

        if (node is IAsteroid asteroid)
        {
            asteroid.Init(cfg.BaseHp, cfg.DropId);
        }

        return node;
    }
}