
using GFramework.Core.Abstractions.utility;

namespace CosmicMiningCompany.scripts.storage;

public interface IWriteStorageUtility: IUtility,IStorageUtility
{
    /// <summary>
    /// 将内容写入存储中的指定键
    /// </summary>
    /// <param name="key">要写入的键</param>
    /// <param name="content">要写入的内容</param>
    void Write(string key, string content);
}