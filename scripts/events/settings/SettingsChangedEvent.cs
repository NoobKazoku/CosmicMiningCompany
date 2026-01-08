using CosmicMiningCompany.scripts.enums;

namespace CosmicMiningCompany.scripts.events.settings;

/// <summary>
/// 表示设置发生更改时触发的事件
/// </summary>
public sealed class SettingsChangedEvent
{
    /// <summary>
    /// 获取设置更改的原因
    /// </summary>
    public SettingsChangedReason Reason { get; set; } 
}