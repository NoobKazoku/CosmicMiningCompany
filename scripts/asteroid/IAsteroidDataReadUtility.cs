using System.Collections.Generic;
using GFramework.Core.Abstractions.utility;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星数据读取工具接口，提供小行星数据的加载和访问功能
/// </summary>
public interface IAsteroidDataReadUtility : IContextUtility
{
    /// <summary>
    /// 当前读取的小行星数据
    /// </summary>
    public AsteroidData? Current { get; }

    /// <summary>
    /// 加载小行星数据
    /// </summary>
    void Load();
    
    /// <summary>
    /// 获取小行星定义字典
    /// </summary>
    /// <returns>以ID为键的小行星定义字典</returns>
    Dictionary<int, AsteroidDefinition> GetAsteroidDefs();
}
