using CosmicMiningCompany.scripts.serializer;
using CosmicMiningCompany.scripts.setting.interfaces;
using CosmicMiningCompany.scripts.storage;
using GFramework.Core.extensions;
using GFramework.Core.utility;
using Godot;

namespace CosmicMiningCompany.scripts.setting;

/// <summary>
/// 设置数据存储工具类，负责设置数据的加载和保存
/// </summary>
public class SettingsStorageUtility: AbstractContextUtility, ISettingsStorageUtility
{
    /// <summary>
    /// 设置文件的路径，保存在用户目录下的setting.json文件
    /// 使用user://路径，在导出后的环境中也能正常工作
    /// </summary>
    private const string Path = "user://setting.json";
    
    private ISaveStorage _storage = null!;
    private readonly ISerializer<SettingsData> _serializer = new SettingsSerializer();
    
    /// <summary>
    /// 初始化存储工具，获取保存存储接口实例
    /// </summary>
    protected override void OnInit()
    {
        _storage = this.GetUtility<ISaveStorage>()!;
    }
    
    /// <summary>
    /// 加载设置数据
    /// </summary>
    /// <returns>设置数据对象，如果文件不存在则返回新的默认设置数据</returns>
    public SettingsData Load()
    {
        return !_storage.Exists(Path) ? new SettingsData() : _serializer.Deserialize(_storage.Read(Path));
    }

    /// <summary>
    /// 保存设置数据到存储文件
    /// </summary>
    /// <param name="data">要保存的设置数据</param>
    public void Save(SettingsData data)
    {
        _storage.Write(Path, _serializer.Serialize(data));
    }
}
