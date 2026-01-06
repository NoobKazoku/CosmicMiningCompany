using GFramework.Core.Abstractions.utility;

namespace CosmicMiningCompany.scripts.data;

/// <summary>
/// 游戏存档数据管理接口，提供游戏进度、技能、场景解锁状态和物品数量的管理功能
/// </summary>
public interface ISaveDataUtility: IUtility
{
    /// <summary>
    /// 检查是否存在已保存的游戏数据
    /// </summary>
    /// <returns>如果存在存档返回true，否则返回false</returns>
    bool HasSave();
    
    /// <summary>
    /// 开始新游戏，初始化新的游戏数据
    /// </summary>
    void NewGame();
    
    /// <summary>
    /// 从存档文件中加载游戏数据
    /// </summary>
    void Load();
    
    /// <summary>
    /// 将当前游戏数据保存到存档文件
    /// </summary>
    void Save();
    
    /// <summary>
    /// 获取指定技能的等级
    /// </summary>
    /// <param name="skillId">技能的唯一标识符</param>
    /// <returns>技能的当前等级</returns>
    int GetSkillLevel(string skillId);
    
    /// <summary>
    /// 设置指定技能的等级
    /// </summary>
    /// <param name="skillId">技能的唯一标识符</param>
    /// <param name="level">要设置的技能等级</param>
    void SetSkillLevel(string skillId, int level);
    
    /// <summary>
    /// 检查指定场景是否已解锁
    /// </summary>
    /// <param name="sceneId">场景的唯一标识符</param>
    /// <returns>如果场景已解锁返回true，否则返回false</returns>
    bool IsSceneUnlocked(string sceneId);
    
    /// <summary>
    /// 解锁指定场景
    /// </summary>
    /// <param name="sceneId">场景的唯一标识符</param>
    void UnlockScene(string sceneId);
    
    /// <summary>
    /// 获取指定物品的数量
    /// </summary>
    /// <param name="itemId">物品的唯一标识符</param>
    /// <returns>物品的当前数量</returns>
    int GetItemCount(string itemId);
    
    /// <summary>
    /// 添加指定数量的物品到库存
    /// </summary>
    /// <param name="itemId">物品的唯一标识符</param>
    /// <param name="count">要添加的物品数量</param>
    void AddItem(string itemId, int count);
    
    /// <summary>
    /// 消耗指定数量的物品
    /// </summary>
    /// <param name="itemId">物品的唯一标识符</param>
    /// <param name="count">要消耗的物品数量</param>
    /// <returns>如果物品数量足够并成功消耗返回true，否则返回false</returns>
    bool ConsumeItem(string itemId, int count);

    void PrintSaveSummary();
}
