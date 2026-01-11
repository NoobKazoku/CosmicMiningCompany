using GFramework.Core.Abstractions.model;

namespace CosmicMiningCompany.scripts.game;

/// <summary>
/// 游戏状态模型接口，定义了游戏运行时的基本状态属性
/// 继承自IModel接口，提供游戏暂停和游戏进行状态的管理
/// </summary>
public interface IGameStateModel: IModel
{
    /// <summary>
    /// 获取游戏是否处于暂停状态
    /// </summary>
    /// <returns>返回true表示游戏已暂停，false表示游戏未暂停</returns>
    bool IsGamePaused();

    /// <summary>
    /// 获取游戏是否正在运行状态
    /// </summary>
    /// <returns>返回true表示游戏正在运行，false表示游戏未在运行</returns>
    bool IsGaming();
    
    /// <summary>
    /// 设置游戏暂停状态
    /// </summary>
    /// <param name="isPaused">要设置的暂停状态，true为暂停，false为继续</param>
    void SetGamePaused(bool isPaused);
    
    /// <summary>
    /// 设置游戏运行状态
    /// </summary>
    /// <param name="isGaming">要设置的运行状态，true为运行，false为停止</param>
    void SetGaming(bool isGaming);
}

