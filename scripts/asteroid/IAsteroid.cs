namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星接口，定义了小行星对象必须实现的基本功能
/// </summary>
public interface IAsteroid
{
    /// <summary>
    /// 初始化小行星对象
    /// </summary>
    /// <param name="definition">小行星定义对象，包含小行星的配置信息</param>
    void Init(AsteroidDefinition definition);
}
