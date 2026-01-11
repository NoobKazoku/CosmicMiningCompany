using GFramework.Core.model;

namespace CosmicMiningCompany.scripts.game;

/// <summary>
/// 游戏状态模型类，用于管理游戏的暂停和进行状态
/// 继承自AbstractModel并实现IGameStateModel接口
/// </summary>
public class GameStateModel: AbstractModel,IGameStateModel
{
    /// <summary>
    /// 游戏状态实例，存储当前游戏的暂停和进行状态
    /// </summary>
    private readonly GameState _gameState = new();
    
    /// <summary>
    /// 初始化方法，重写父类的OnInit方法
    /// </summary>
    protected override void OnInit()
    {
        
    }

    /// <summary>
    /// 获取游戏是否处于暂停状态
    /// </summary>
    /// <returns>如果游戏已暂停则返回true，否则返回false</returns>
    public bool IsGamePaused()
    {
        return _gameState.IsPaused;
    }

    /// <summary>
    /// 获取游戏是否处于进行中状态
    /// </summary>
    /// <returns>如果游戏正在进行则返回true，否则返回false</returns>
    public bool IsGaming()
    {
        return _gameState.IsGaming;
    }

    /// <summary>
    /// 设置游戏暂停状态
    /// </summary>
    /// <param name="isPaused">要设置的暂停状态，true表示暂停，false表示继续</param>
    public void SetGamePaused(bool isPaused)
    {
        _gameState.IsPaused = isPaused;
    }

    /// <summary>
    /// 设置游戏进行状态
    /// </summary>
    /// <param name="isGaming">要设置的游戏状态，true表示游戏中，false表示非游戏状态</param>
    public void SetGaming(bool isGaming)
    {
        _gameState.IsGaming = isGaming;
    }
}
