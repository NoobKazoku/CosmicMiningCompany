using System.Collections.Generic;
using System.IO;
using System.Linq;
using CosmicMiningCompany.scripts.constants;
using GFramework.Core.Abstractions.environment;
using GFramework.Core.extensions;
using GFramework.Core.utility;
using Godot;
using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.data;

/// <summary>
/// 保存数据工具类，负责游戏存档的创建、保存、加载和检查功能
/// </summary>
public class SaveDataUtility : AbstractContextUtility, ISaveDataUtility
{
    /// <summary>
    /// 存档文件的路径，保存在用户目录下的save.json文件
    /// </summary>
    private static readonly string SavePath =
        ProjectSettings.GlobalizePath(
            ProjectSettings.GetSetting("application/save/save_path").AsString()
        );

    /// <summary>
    /// 检查是否存在存档文件
    /// </summary>
    /// <returns>如果存档文件存在返回true，否则返回false</returns>
    public static bool HasSave() => File.Exists(SavePath);

    private static GameSaveData? _current;

    protected override void OnInit()
    {
        if (_current == null)
        {
            Load();
        }
    }

    bool ISaveDataUtility.HasSave()
    {
        return HasSave();
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

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SavePath, json);

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
        if (!HasSave())
        {
            _current = new GameSaveData();
            return;
        }

        var json = File.ReadAllText(SavePath);
        var deserializedData = JsonConvert.DeserializeObject<GameSaveData>(json);
        _current = deserializedData ?? new GameSaveData();
        _current.RuntimeDirty = false;
    }

    /// <summary>
    /// 标记当前存档数据为脏数据，需要保存
    /// </summary>
    private static void MarkDirty()
    {
        _current?.RuntimeDirty = true;
    }

    /// <summary>
    /// 确保存档数据已加载，如果未加载则从文件加载或创建新存档
    /// </summary>
    /// <returns>当前加载的存档数据</returns>
    private static GameSaveData EnsureLoaded()
    {
        if (_current != null) return _current;
        if (HasSave())
        {
            var json = File.ReadAllText(SavePath);
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