using GFramework.Core.Abstractions.utility;

namespace CosmicMiningCompany.scripts.storage;

/// <summary>
/// 存储读取工具接口，定义从存储系统读取数据的基本操作
/// </summary>
public interface IReadStorageUtility: IUtility,IStorageUtility
{
    /// <summary>
    /// 从存储中读取指定键对应的值
    /// </summary>
    /// <param name="key">要读取的键</param>
    /// <returns>键对应的值内容</returns>
    string Read(string key);
}
