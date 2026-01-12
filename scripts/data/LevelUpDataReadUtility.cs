using CosmicMiningCompany.scripts.serializer;
using CosmicMiningCompany.scripts.storage;
using GFramework.Core.extensions;
using GFramework.Core.utility;

namespace CosmicMiningCompany.scripts.data;

/// <summary>
/// 技能升级数据读取工具类，负责从存储中读取技能升级数据并反序列化
/// </summary>
public class LevelUpDataReadUtility : AbstractContextUtility
{
    /// <summary>
    /// LevelUp数据文件的路径
    /// 直接使用res://路径，在导出后的环境中也能正常工作
    /// </summary>
    private const string LevelUpPath = "res://assets/data/LevelUp.json";

    
    /// <summary>
    /// LevelUp数据序列化器，用于将JSON数据反序列化为SkillRoot对象
    /// </summary>
    private readonly ISerializer<SkillRoot> _serializer = new LevelUpDataSerializer();
    
    /// <summary>
    /// 当前读取的技能数据
    /// </summary>
    public SkillRoot? Current { get; private set; }
    
    /// <summary>
    /// 存储读取工具，用于从文件系统读取数据
    /// </summary>
    private IReadStorageUtility _storage = null!;
    
    /// <summary>
    /// 初始化方法，获取存储读取工具实例
    /// </summary>
    protected override void OnInit()
    {
        _storage = this.GetUtility<IReadStorageUtility>()!;
        Load();
    }
    
    /// <summary>
    /// 从存储中读取LevelUp数据并反序列化到Current属性中
    /// </summary>
    public void Load()
    {
        var json = _storage.Read(LevelUpPath);
        Current = _serializer.Deserialize(json);
    }
    
    /// <summary>
    /// 根据技能名称获取技能数据
    /// </summary>
    /// <param name="skillName">技能名称</param>
    /// <returns>技能数据，如果未找到则返回null</returns>
    public SkillData? GetSkillData(string skillName)
    {
        if (Current == null || Current.Skills == null)
            return null;
            
        return Current.Skills.Find(skill => skill.Name == skillName);
    }
    
    /// <summary>
    /// 根据技能名称和等级获取等级数据
    /// </summary>
    /// <param name="skillName">技能名称</param>
    /// <param name="level">等级</param>
    /// <returns>等级数据，如果未找到则返回null</returns>
    public SkillLevel? GetSkillLevelData(string skillName, int level)
    {
        var skillData = GetSkillData(skillName);
        if (skillData == null || skillData.Levels == null)
            return null;
            
        return skillData.Levels.Find(l => l.Level == level);
    }
}
