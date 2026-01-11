namespace CosmicMiningCompany.scripts.game;

/// <summary>
/// 表示游戏状态的不可变类，用于管理游戏的核心状态信息
/// </summary>
public sealed class GameState
{
    /// <summary>
    /// 获取或设置游戏是否正在运行的状态
    /// </summary>
    public bool IsGaming { get; set; }
    
    /// <summary>
    /// 获取或设置游戏是否处于暂停状态
    /// </summary>
    public bool IsPaused { get; set; }
}
