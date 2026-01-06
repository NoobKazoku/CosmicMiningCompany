using System.IO;

namespace CosmicMiningCompany.scripts.storage;

/// <summary>
/// 文件保存存储实现类，提供基于文件系统的保存存储功能
/// </summary>
public class FileSaveStorage : ISaveStorage
{
    /// <summary>
    /// 检查指定路径的文件是否存在
    /// </summary>
    /// <param name="key">要检查的文件路径</param>
    /// <returns>如果文件存在返回true，否则返回false</returns>
    public bool Exists(string key) => File.Exists(key);

    /// <summary>
    /// 读取指定路径文件的内容
    /// </summary>
    /// <param name="key">要读取的文件路径</param>
    /// <returns>文件的内容字符串</returns>
    public string Read(string key) => File.ReadAllText(key);

    /// <summary>
    /// 将内容写入指定路径的文件
    /// </summary>
    /// <param name="key">要写入的文件路径</param>
    /// <param name="content">要写入的文件内容</param>
    public void Write(string key, string content)
    {
        // 创建文件所在目录（如果不存在）
        Directory.CreateDirectory(Path.GetDirectoryName(key)!);
        File.WriteAllText(key, content);
    }
}

