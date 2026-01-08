using GFramework.Core.Abstractions.utility;

namespace CosmicMiningCompany.scripts.storage;

/// <summary>
/// 定义保存存储接口，提供基本的键值对存储操作功能
/// </summary>
public interface ISaveStorage: IUtility
{
    /// <summary>
    /// 检查指定键是否存在于存储中
    /// </summary>
    /// <param name="key">要检查的键</param>
    /// <returns>如果键存在则返回true，否则返回false</returns>
    bool Exists(string key);
    
    /// <summary>
    /// 从存储中读取指定键对应的值
    /// </summary>
    /// <param name="key">要读取的键</param>
    /// <returns>键对应的值内容</returns>
    string Read(string key);
    
    /// <summary>
    /// 将内容写入存储中的指定键
    /// </summary>
    /// <param name="key">要写入的键</param>
    /// <param name="content">要写入的内容</param>
    void Write(string key, string content);
}

