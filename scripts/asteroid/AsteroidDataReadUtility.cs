using System.Collections.Generic;
using System.Linq;
using CosmicMiningCompany.scripts.serializer;
using CosmicMiningCompany.scripts.storage;
using GFramework.Core.extensions;
using GFramework.Core.utility;
using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星数据读取工具类，负责从存储中读取小行星数据并反序列化
/// </summary>
public class AsteroidDataReadUtility : AbstractContextUtility, IAsteroidDataReadUtility
{
    /// <summary>
    /// 小行星数据文件的路径，从项目设置中获取并全局化路径
    /// </summary>
    private static readonly string AsteroidPath =
        ProjectSettings.GlobalizePath(
            ProjectSettings.GetSetting("application/config/assets/asteroid_path").AsString()
        );

    /// <summary>
    /// 小行星数据序列化器，用于将JSON数据反序列化为AsteroidData对象
    /// </summary>
    private readonly ISerializer<AsteroidData> _serializer = new AsteroidDataSerializer();

    private Dictionary<int, AsteroidDefinition> _defs = new();

    /// <summary>
    /// 当前读取的小行星数据
    /// </summary>
    public AsteroidData? Current { get; private set; }

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
    /// 从存储中读取小行星数据并反序列化到Current属性中
    /// </summary>
    public void Load()
    {
        var json = _storage.Read(AsteroidPath);
        Current = _serializer.Deserialize(json);
        _defs = Current.Definitions.ToDictionary(d => d.Id);
    }

    public Dictionary<int, AsteroidDefinition> GetAsteroidDefs()
    {
        return _defs;
    }
}