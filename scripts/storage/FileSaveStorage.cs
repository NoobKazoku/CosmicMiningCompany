using System.IO;
using FileAccess = Godot.FileAccess;

namespace CosmicMiningCompany.scripts.storage;

/// <summary>
/// 文件保存存储实现类，提供基于文件系统的保存存储功能
/// 支持Godot特殊路径（res://和user://）以及普通文件路径
/// </summary>
public class FileSaveStorage : ISaveStorage
{
    /// <summary>
    /// 检查指定路径的文件是否存在
    /// </summary>
    /// <param name="key">要检查的文件路径</param>
    /// <returns>如果文件存在返回true，否则返回false</returns>
    public bool Exists(string key)
    {
        // 处理普通文件路径
        if (!key.StartsWith("res://") && !key.StartsWith("user://")) return File.Exists(key);
        // 处理Godot特殊路径
        using var file = FileAccess.Open(key, FileAccess.ModeFlags.Read);
        return file != null;
    }

    /// <summary>
    /// 读取指定路径文件的内容
    /// </summary>
    /// <param name="key">要读取的文件路径</param>
    /// <returns>文件的内容字符串</returns>
    public string Read(string key)
    {
        // 处理Godot特殊路径
        if (!key.StartsWith("res://") && !key.StartsWith("user://")) return File.ReadAllText(key);
        using var file = FileAccess.Open(key, FileAccess.ModeFlags.Read);
        // 处理普通文件路径
        return file != null ? file.GetAsText() : throw new FileNotFoundException($"无法读取文件: {key}");
    }

    /// <summary>
    /// 将内容写入指定路径的文件
    /// </summary>
    /// <param name="key">要写入的文件路径</param>
    /// <param name="content">要写入的文件内容</param>
    public void Write(string key, string content)
    {
        // 处理Godot特殊路径
        if (key.StartsWith("res://") || key.StartsWith("user://"))
        {
            using var file = FileAccess.Open(key, FileAccess.ModeFlags.Write);
            if (file == null) throw new IOException($"无法写入文件: {key}");
            file.StoreString(content);
            return;
        }
        
        // 处理普通文件路径
        // 创建文件所在目录（如果不存在）
        Directory.CreateDirectory(Path.GetDirectoryName(key)!);
        File.WriteAllText(key, content);
    }
}
