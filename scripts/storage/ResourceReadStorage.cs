using System.IO;
using Godot;

namespace CosmicMiningCompany.scripts.storage;

/// <summary>
/// 资源读取存储实现类，提供基于Godot资源系统的读取功能
/// 支持在编辑器和导出后的环境中读取资源文件
/// </summary>
public class ResourceReadStorage : IReadStorageUtility
{
    /// <summary>
    /// 检查指定路径的资源是否存在
    /// </summary>
    /// <param name="key">要检查的资源路径</param>
    /// <returns>如果资源存在返回true，否则返回false</returns>
    public bool Exists(string key)
    {
        // 对于资源路径，使用ResourceLoader.Exists检查
        if (key.StartsWith("res://"))
        {
            return ResourceLoader.Exists(key);
        }
        
        // 对于普通文件路径，使用File.Exists检查
        return File.Exists(key);
    }

    /// <summary>
    /// 读取指定路径资源的内容
    /// </summary>
    /// <param name="key">要读取的资源路径</param>
    /// <returns>资源的内容字符串</returns>
    public string Read(string key)
    {
        // 对于资源路径，使用Godot的FileAccess读取
        if (key.StartsWith("res://"))
        {
            // 使用Godot.FileAccess来避免歧义
            using var file = Godot.FileAccess.Open(key, Godot.FileAccess.ModeFlags.Read);
            if (file != null)
            {
                return file.GetAsText();
            }
            
            throw new FileNotFoundException($"无法读取资源文件: {key}");
        }
        
        // 对于普通文件路径，使用File.ReadAllText读取
        return File.ReadAllText(key);
    }
}
