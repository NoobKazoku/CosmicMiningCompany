using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星生成规则类，根据给定的小行星数据决定在指定距离处生成哪种类型的小行星
/// </summary>
/// <param name="data">包含小行星定义和生成规则的数据对象</param>
public class AsteroidSpawnRule(AsteroidData data)
{
    /// <summary>
    /// 判断在指定距离处是否可以生成小行星
    /// </summary>
    /// <param name="distance">当前生成位置的距离</param>
    /// <returns>如果存在允许在此距离生成的规则则返回true，否则返回false</returns>
    public bool CanSpawn(float distance)
    {
        return data.SpawnRules.Any(r => distance >= r.MinDistance);
    }

    /// <summary>
    /// 根据距离和权重规则决定生成哪个ID的小行星
    /// </summary>
    /// <param name="distance">当前生成位置的距离</param>
    /// <returns>选中的小行星ID</returns>
    public int DecideAsteroidId(float distance)
    {
        var candidates = new List<(int id, float weight)>();

        foreach (var rule in data.SpawnRules)
        {
            if (distance < rule.MinDistance || distance > rule.MaxDistance)
                continue;

            var weight =
                rule.BaseWeight +
                rule.DistanceFactor * (distance - rule.MinDistance);

            if (weight > 0)
                candidates.Add((rule.AsteroidId, weight));
        }

        return Roll(candidates);
    }

    /// <summary>
    /// 根据权重随机选择一个小行星ID
    /// </summary>
    /// <param name="list">包含小行星ID和对应权重的列表</param>
    /// <returns>通过加权随机选择的小行星ID</returns>
    private static int Roll(List<(int id, float weight)> list)
    {
        var total = list.Sum(x => x.weight);
        var roll = GD.Randf() * total;

        float acc = 0;
        foreach (var (id, w) in list)
        {
            acc += w;
            if (roll <= acc)
                return id;
        }

        throw new InvalidOperationException("No asteroid rolled");
    }
}
