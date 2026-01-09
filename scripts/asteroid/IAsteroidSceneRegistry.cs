using Godot;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星场景注册表接口，用于根据键值获取打包的场景资源
/// </summary>
public interface IAsteroidSceneRegistry
{
    /// <summary>
    /// 根据指定的键获取打包的场景
    /// </summary>
    /// <param name="key">用于标识场景的键值</param>
    /// <returns>与键关联的打包场景对象</returns>
    PackedScene Get(string key);
}
