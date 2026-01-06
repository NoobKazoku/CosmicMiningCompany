using GFramework.Core.Abstractions.system;

namespace CosmicMiningCompany.scripts.setting.interfaces;

/// <summary>
/// 定义设置系统的接口，提供应用各种设置的方法
/// </summary>
public interface ISettingsSystem: ISystem
{
    /// <summary>
    /// 应用所有设置项
    /// </summary>
    void ApplyAll();

    /// <summary>
    /// 应用图形设置
    /// </summary>
    void ApplyGraphics();
    
    /// <summary>
    /// 应用音频设置
    /// </summary>
    void ApplyAudio();
}
