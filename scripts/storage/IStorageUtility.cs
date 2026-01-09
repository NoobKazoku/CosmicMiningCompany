namespace CosmicMiningCompany.scripts.storage;

/// <summary>
/// 存储工具接口，定义了存储操作的基本方法
/// </summary>
public interface IStorageUtility
{
    /// <summary>
    /// 检查指定键是否存在于存储中
    /// </summary>
    /// <param name="key">要检查的键</param>
    /// <returns>如果键存在则返回true，否则返回false</returns>
    bool Exists(string key);
}
