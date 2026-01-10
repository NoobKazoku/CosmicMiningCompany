using System;
using CosmicMiningCompany.scripts.setting.interfaces;
using CosmicMiningCompany.scripts.storage;
using GFramework.Core.extensions;
using GFramework.Core.utility;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;
using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.setting;

/// <summary>
/// 设置数据存储工具类，负责设置数据的加载和保存
/// </summary>
[ContextAware]
[Log]
public partial class SettingsStorageUtility: AbstractContextUtility, ISettingsStorageUtility
{
    /// <summary>
    /// 存档文件的路径，保存在用户目录下的save.json文件
    /// </summary>
    private static readonly string Path =
        ProjectSettings.GlobalizePath(
            ProjectSettings.GetSetting("application/config/save/setting_path").AsString()
        );
    private ISaveStorage _storage = null!;
    
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
    /// <returns>设置数据对象，如果文件不存在或加载失败则返回新的默认设置数据</returns>
    public SettingsData Load()
    {
        try
        {
            if (!_storage.Exists(Path))
            {
                _log.Info("设置文件不存在，返回默认设置");
                return new SettingsData();
            }
            
            string jsonContent = _storage.Read(Path);
            if (string.IsNullOrEmpty(jsonContent))
            {
                _log.Warn("设置文件为空，返回默认设置");
                return new SettingsData();
            }
            
            SettingsData? settings = JsonConvert.DeserializeObject<SettingsData>(jsonContent);
            if (settings == null)
            {
                _log.Error("设置文件反序列化失败，返回默认设置");
                return new SettingsData();
            }
            
            _log.Debug("设置加载成功");
            return settings;
        } catch (Exception ex)
        {
            _log.Error($"加载设置时发生错误: {ex.Message}");
            return new SettingsData();
        }
    }

    /// <summary>
    /// 保存设置数据到存储文件
    /// </summary>
    /// <param name="data">要保存的设置数据</param>
    /// <exception cref="ArgumentNullException">当数据为null时抛出</exception>
    public void Save(SettingsData data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        
        try
        {
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.Indented);
            _storage.Write(Path, jsonContent);
            _log.Debug("设置保存成功");
        }
        catch (Exception ex)
        {
            _log.Error($"保存设置时发生错误: {ex.Message}");
            throw;
        }
    }
}
