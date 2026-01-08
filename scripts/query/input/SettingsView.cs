namespace CosmicMiningCompany.scripts.query.input;

/// <summary>
/// 表示游戏设置视图的数据模型，包含音频和显示设置信息
/// </summary>
public sealed class SettingsView
{
    /// <summary>
    /// 获取主音量设置值
    /// </summary>
    public float MasterVolume { get; init; }
    
    /// <summary>
    /// 获取背景音乐音量设置值
    /// </summary>
    public float BgmVolume { get; init; }
    
    /// <summary>
    /// 获取音效音量设置值
    /// </summary>
    public float SfxVolume { get; init; }

    /// <summary>
    /// 获取全屏显示设置
    /// </summary>
    public bool Fullscreen { get; init; }
    
    /// <summary>
    /// 获取分辨率宽度设置
    /// </summary>
    public int ResolutionWidth { get; init; }
    
    /// <summary>
    /// 获取分辨率高度设置
    /// </summary>
    public int ResolutionHeight { get; init; }
}

