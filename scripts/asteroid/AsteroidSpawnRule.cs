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
    /// 判断在指定距离处是否存在「有效的生成规则」
    /// </summary>
    /// <param name="distance">距离值</param>
    /// <returns>如果存在有效生成规则则返回true，否则返回false</returns>
    public bool CanSpawn(float distance)
    {
        return data.SpawnRules.Any(rule =>
            distance >= rule.MinDistance &&
            distance <= rule.MaxDistance &&
            rule.BaseWeight +
            rule.DistanceFactor * (distance - rule.MinDistance) > 0
        );
    }

    /// <summary>
    /// 根据距离和权重规则决定生成哪个小行星ID
    /// 返回 -1 表示本次不生成
    /// </summary>
    /// <param name="distance">距离值</param>
    /// <returns>返回选中的小行星ID，如果无有效规则则返回-1</returns>
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
    /// 加权随机选择函数
    /// </summary>
    /// <param name="list">包含ID和权重的候选列表</param>
    /// <returns>返回选中的ID，如果列表为空或总权重小于等于0则返回-1</returns>
    private static int Roll(List<(int id, float weight)> list)
    {
        if (list.Count == 0)
            return -1;

        var total = list.Sum(x => x.weight);
        if (total <= 0)
            return -1;

        var roll = GD.Randf() * total;

        float acc = 0;
        foreach (var (id, w) in list)
        {
            acc += w;
            if (roll <= acc)
                return id;
        }

        // 浮点误差兜底
        return list[^1].id;
    }
}
