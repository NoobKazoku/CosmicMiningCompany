using GFramework.Core.Abstractions.utility;

namespace CosmicMiningCompany.scripts.asteroid;

public interface IAsteroidDataReadUtility : IContextUtility
{
    /// <summary>
    /// 当前读取的小行星数据
    /// </summary>
    public AsteroidData? Current { get; }

    void Load();
}