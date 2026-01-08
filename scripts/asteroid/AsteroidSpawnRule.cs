using CosmicMiningCompany.scripts.enums;
using Godot;
namespace CosmicMiningCompany.scripts.asteroid;


/// <summary>
/// 小行星生成规则类，根据距离配置决定是否生成小行星以及生成的类型
/// </summary>
/// <param name="config">小行星生成配置对象</param>
public class AsteroidSpawnRule(AsteroidSpawnConfig config)
{
    /// <summary>
    /// 判断在指定距离处是否可以生成小行星
    /// </summary>
    /// <param name="distance">生成位置的距离</param>
    /// <returns>如果距离大于等于正常开始距离则返回true，否则返回false</returns>
    public bool CanSpawn(float distance)
    {
        return distance >= config.NormalStartDistance;
    }

    /// <summary>
    /// 根据距离决定生成的小行星类型
    /// </summary>
    /// <param name="distance">生成位置的距离</param>
    /// <returns>返回AsteroidType枚举值，根据权重随机决定是普通小行星还是蓝宝石小行星</returns>
    public AsteroidType Decide(float distance)
    {
        const float normalWeight = 1f;
        var gemWeight = 0f;

        // 计算宝石小行星的权重，当距离达到宝石开始距离时启用
        if (distance >= config.GemStartDistance)
        {
            gemWeight = Mathf.Clamp(
                config.GemWeightFactor * (distance - config.GemStartDistance),
                0f,
                config.GemMaxWeight
            );
        }

        var total = normalWeight + gemWeight;
        var roll = GD.Randf() * total;

        return roll < normalWeight
            ? AsteroidType.Normal
            : AsteroidType.Sapphire;
    }
}
