using System.Collections.Generic;
using System.IO;
using System.Linq;
using CosmicMiningCompany.scripts.constants;
using CosmicMiningCompany.scripts.data.interfaces;
using CosmicMiningCompany.scripts.serializer;
using CosmicMiningCompany.scripts.storage;
using GFramework.Core.Abstractions.environment;
using GFramework.Core.extensions;
using GFramework.Core.utility;
using Godot;
using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.data;

/// <summary>
/// 保存数据工具类，负责游戏存档的创建、保存、加载和检查功能
/// </summary>
public class SaveStorageUtility : AbstractContextUtility, ISaveStorageUtility
{
    /// <summary>
    /// 存档文件的路径，保存在用户目录下的save.json文件
    /// </summary>
    private const string SavePath = "user://save.json";

    private readonly ISerializer<GameSaveData> _serializer = new GameSaveSerializer();
    private  GameSaveData? _current;

    private ISaveStorage _storage = null!;

    protected override void OnInit()
    {
        _storage = this.GetUtility<ISaveStorage>()!;
    }

    bool ISaveStorageUtility.HasSave()
    {
        return _storage.Exists(SavePath);
    }

    /// <summary>
    /// 开始新游戏，创建新的存档数据并保存
    /// </summary>
    public void NewGame()
    {
        _current = new GameSaveData
        {
            RuntimeDirty = true
        };
        Save();
    }

    /// <summary>
    /// 将当前存档数据保存到文件
    /// </summary>
    public void Save()
    {
        var data = EnsureLoaded();
        if (!data.RuntimeDirty)
        {
            return;
        }

        _storage.Write(SavePath, _serializer.Serialize(data));
        data.RuntimeDirty = false;
    }

    /// <summary>
    /// 获取指定技能的等级
    /// </summary>
    /// <param name="skillId">技能ID</param>
    /// <returns>技能等级，如果技能不存在则返回0</returns>
    public int GetSkillLevel(string skillId)
    {
        var data = EnsureLoaded();
        return data.SkillLevels.GetValueOrDefault(skillId, 0);
    }

    /// <summary>
    /// 设置指定技能的等级
    /// </summary>
    /// <param name="skillId">技能ID</param>
    /// <param name="level">技能等级</param>
    public void SetSkillLevel(string skillId, int level)
    {
        var data = EnsureLoaded();
        data.SkillLevels[skillId] = level;
        MarkDirty();
    }

    /// <summary>
    /// 检查指定场景是否已解锁
    /// </summary>
    /// <param name="sceneId">场景ID</param>
    /// <returns>如果场景已解锁返回true，否则返回false</returns>
    public bool IsSceneUnlocked(string sceneId)
    {
        var data = EnsureLoaded();
        return data.UnlockedScenes.Contains(sceneId);
    }

    /// <summary>
    /// 解锁指定场景
    /// </summary>
    /// <param name="sceneId">场景ID</param>
    public void UnlockScene(string sceneId)
    {
        var data = EnsureLoaded();
        if (data.UnlockedScenes.Contains(sceneId))
            return;

        data.UnlockedScenes.Add(sceneId);
        MarkDirty();
    }

    /// <summary>
    /// 获取指定物品的数量
    /// </summary>
    /// <param name="itemId">物品ID</param>
    /// <returns>物品数量，如果物品不存在则返回0</returns>
    public int GetItemCount(string itemId)
    {
        var data = EnsureLoaded();
        return data.Inventory.GetValueOrDefault(itemId, 0);
    }

    /// <summary>
    /// 添加指定数量的物品到库存
    /// </summary>
    /// <param name="itemId">物品ID</param>
    /// <param name="count">添加的数量</param>
    public void AddItem(string itemId, int count)
    {
        if (count <= 0) return;

        var data = EnsureLoaded();
        data.Inventory.TryGetValue(itemId, out var old);
        data.Inventory[itemId] = old + count;
        MarkDirty();
    }

    /// <summary>
    /// 消耗指定数量的物品
    /// </summary>
    /// <param name="itemId">物品ID</param>
    /// <param name="count">消耗的数量</param>
    /// <returns>如果物品足够消耗返回true，否则返回false</returns>
    public bool ConsumeItem(string itemId, int count)
    {
        var data = EnsureLoaded();
        if (!data.Inventory.TryGetValue(itemId, out var old) || old < count)
            return false;

        var left = old - count;
        if (left <= 0)
            data.Inventory.Remove(itemId);
        else
            data.Inventory[itemId] = left;

        MarkDirty();
        return true;
    }

    /// <summary>
    /// 从文件加载存档数据，如果不存在存档则创建新的存档
    /// </summary>
    public void Load()
    {
        // 检查存档文件是否存在
        if (!_storage.Exists(SavePath))
        {
            _current = new GameSaveData();
            return;
        }

        var json = _storage.Read(SavePath);
        _current = _serializer.Deserialize(json);
        _current.RuntimeDirty = false;
    }

    /// <summary>
    /// 标记当前存档数据为脏数据，需要保存
    /// </summary>
    private  void MarkDirty()
    {
        _current?.RuntimeDirty = true;
    }

    /// <summary>
    /// 确保存档数据已加载，如果未加载则从文件加载或创建新存档
    /// </summary>
    /// <returns>当前加载的存档数据</returns>
    private  GameSaveData EnsureLoaded()
    {
        if (_current != null) return _current;
        if (_storage.Exists(SavePath))
        {
            var json = _storage.Read(SavePath);
            _current = JsonConvert.DeserializeObject<GameSaveData>(json)
                       ?? new GameSaveData();
        }
        else
        {
            _current = new GameSaveData();
        }

        _current.RuntimeDirty = false;

        return _current;
    }

    /// <summary>
    /// 打印存档摘要信息，仅在开发环境中可用
    /// </summary>
    public void PrintSaveSummary()
    {
        var env = this.GetEnvironment<IEnvironment>();
        if (env is null || !GameConstants.Development.Equals(env.Name))
        {
            return;
        }

        var data = EnsureLoaded();

        GD.Print("===== SAVE SUMMARY =====");
        GD.Print($"Version: {data.Version}");
        GD.Print($"Skills: {data.SkillLevels.Count}");
        GD.Print($"Unlocked Scenes: {data.UnlockedScenes.Count}");
        GD.Print($"Inventory Items: {data.Inventory.Count}");
        GD.Print($"Dirty: {data.RuntimeDirty}");
        PrintSkills(data);
        PrintScenes(data);
        PrintInventory(data);
        GD.Print("===== END SAVE SUMMARY =====");
    }

    /// <summary>
    /// 打印技能信息
    /// </summary>
    /// <param name="data">存档数据</param>
    private static void PrintSkills(GameSaveData data)
    {
        GD.Print("-- Skills --");

        if (data.SkillLevels.Count == 0)
        {
            GD.Print("  (none)");
            return;
        }

        foreach (var (id, level) in data.SkillLevels.OrderBy(e => e.Key))
        {
            GD.Print($"  {id,-20} Lv.{level}");
        }
    }

    /// <summary>
    /// 打印已解锁场景信息
    /// </summary>
    /// <param name="data">存档数据</param>
    private static void PrintScenes(GameSaveData data)
    {
        GD.Print("-- Unlocked Scenes --");

        if (data.UnlockedScenes.Count == 0)
        {
            GD.Print("  (none)");
            return;
        }

        foreach (var scene in data.UnlockedScenes.OrderBy(s => s))
        {
            GD.Print($"  {scene}");
        }
    }

    /// <summary>
    /// 打印库存信息
    /// </summary>
    /// <param name="data">存档数据</param>
    private static void PrintInventory(GameSaveData data)
    {
        GD.Print("-- Inventory --");

        if (data.Inventory.Count == 0)
        {
            GD.Print("  (empty)");
            return;
        }

        foreach (var (item, count) in data.Inventory.OrderByDescending(e => e.Value))
        {
            var flag = count <= 0 ? " ⚠" : "";
            GD.Print($"  {item,-20} x{count}{flag}");
        }
    }
}
