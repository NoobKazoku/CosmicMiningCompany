using System.Collections.Generic;
using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.data;

/// <summary>
/// 技能升级数据根对象
/// </summary>
public class SkillRoot
{
    /// <summary>
    /// 技能列表
    /// </summary>
    [JsonProperty("skills")]
    public List<SkillData> Skills { get; set; } = new List<SkillData>();
}

/// <summary>
/// 单个技能的数据
/// </summary>
public class SkillData
{
    /// <summary>
    /// 技能内部名称（英文）
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 技能显示名称（中文）
    /// </summary>
    [JsonProperty("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 技能描述
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 最大等级
    /// </summary>
    [JsonProperty("maxlevel")]
    public int MaxLevel { get; set; }

    /// <summary>
    /// 各等级数据
    /// </summary>
    [JsonProperty("levels")]
    public List<SkillLevel> Levels { get; set; } = new List<SkillLevel>();
}

/// <summary>
/// 技能等级数据
/// </summary>
public class SkillLevel
{
    /// <summary>
    /// 等级
    /// </summary>
    [JsonProperty("level")]
    public int Level { get; set; }

    /// <summary>
    /// 该等级对应的数值
    /// </summary>
    [JsonProperty("value")]
    public float Value { get; set; }

    /// <summary>
    /// 升级消耗
    /// </summary>
    [JsonProperty("upgradeCost")]
    public UpgradeCost UpgradeCost { get; set; } = new UpgradeCost();
}

/// <summary>
/// 升级消耗
/// </summary>
public class UpgradeCost
{
    /// <summary>
    /// 散矿消耗
    /// </summary>
    [JsonProperty("ore")]
    public int Ore { get; set; }

    /// <summary>
    /// 宝石消耗
    /// </summary>
    [JsonProperty("gem")]
    public int Gem { get; set; }
}

/// <summary>
/// LevelUp数据静态工具类，参考陨石的AsteroidDataHelper实现
/// </summary>
public static class LevelUpDataHelper
{
    /// <summary>
    /// 从JSON字符串加载技能数据
    /// </summary>
    /// <param name="json">JSON字符串</param>
    /// <returns>SkillRoot对象</returns>
    public static SkillRoot LoadFromJson(string json)
    {
        return JsonConvert.DeserializeObject<SkillRoot>(json) ?? new SkillRoot();
    }
    
    /// <summary>
    /// 根据技能名称获取技能数据
    /// </summary>
    /// <param name="root">技能根对象</param>
    /// <param name="skillName">技能名称</param>
    /// <returns>技能数据，如果未找到则返回null</returns>
    public static SkillData? GetSkillData(SkillRoot root, string skillName)
    {
        if (root == null || root.Skills == null)
            return null;
            
        return root.Skills.Find(skill => skill.Name == skillName);
    }
    
    /// <summary>
    /// 根据技能名称和等级获取等级数据
    /// </summary>
    /// <param name="root">技能根对象</param>
    /// <param name="skillName">技能名称</param>
    /// <param name="level">等级</param>
    /// <returns>等级数据，如果未找到则返回null</returns>
    public static SkillLevel? GetSkillLevelData(SkillRoot root, string skillName, int level)
    {
        var skillData = GetSkillData(root, skillName);
        if (skillData == null || skillData.Levels == null)
            return null;
            
        return skillData.Levels.Find(l => l.Level == level);
    }
    
    /// <summary>
    /// 获取所有技能名称列表
    /// </summary>
    /// <param name="root">技能根对象</param>
    /// <returns>技能名称列表</returns>
    public static List<string> GetAllSkillNames(SkillRoot root)
    {
        var names = new List<string>();
        if (root == null || root.Skills == null)
            return names;
            
        foreach (var skill in root.Skills)
        {
            names.Add(skill.Name);
        }
        return names;
    }
}
