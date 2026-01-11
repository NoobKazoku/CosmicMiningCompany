using CosmicMiningCompany.scripts.enums;

namespace CosmicMiningCompany.scripts.events.audio;

/// <summary>
/// 背景音乐变更事件类
/// 用于表示背景音乐类型发生变化的事件
/// </summary>
public sealed class BgmChangedEvent
{
    /// <summary>
    /// 获取背景音乐类型
    /// </summary>
    public BgmType BgmType { get; init; }
}