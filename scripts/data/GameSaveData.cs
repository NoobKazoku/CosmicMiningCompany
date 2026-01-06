using System.Collections.Generic;
using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.data;

/// <summary>
/// 游戏存档数据类，用于存储玩家的游戏进度和状态信息
/// </summary>
public class GameSaveData
{
    /// <summary>
    /// 存档版本号，用于处理不同版本间的兼容性
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// 玩家技能等级字典，键为技能名称，值为对应等级
    /// </summary>
    public readonly Dictionary<string, int> SkillLevels = new();
    
    /// <summary>
    /// 已解锁场景列表，存储场景名称
    /// </summary>
    public readonly List<string> UnlockedScenes = [];
    
    /// <summary>
    /// 玩家物品栏字典，键为物品名称，值为对应数量
    /// </summary>
    public readonly Dictionary<string, int> Inventory = new();

    /// <summary>
    /// 运行时脏标记，用于标识存档数据是否被修改过
    /// </summary>
    [JsonIgnore]
    public bool RuntimeDirty { get; set; }
}

