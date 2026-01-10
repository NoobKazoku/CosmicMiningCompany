
using CosmicMiningCompany.scripts.core;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

public class AsteroidPoolSystem
    : AbstractNodePool<SpaceRock>, IAsteroidPoolSystem
{
    protected override PackedScene LoadScene()
    {

    }

    public SpaceRock Acquire(Node2D parent)
    {
        throw new System.NotImplementedException();
    }
}