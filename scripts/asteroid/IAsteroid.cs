namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星接口，定义了小行星对象的基本初始化方法
/// </summary>
public interface IAsteroid
{
    /// <summary>
    /// 初始化小行星对象
    /// </summary>
    /// <param name="hp">小行星的生命值</param>
    /// <param name="dropId">小行星被破坏后掉落物品的ID</param>
    void Init(float hp, string dropId);
}

