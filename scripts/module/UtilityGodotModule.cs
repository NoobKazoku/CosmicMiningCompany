using GFramework.Core.Abstractions.architecture;
using GFramework.Game.architecture;

namespace CosmicMiningCompany.scripts.module;



/// <summary>
/// Godot工具模块类，负责安装和管理游戏中的实用工具组件
/// </summary>
public class UtilityGodotModule: AbstractModule
{
    /// <summary>
    /// 安装模块到指定的游戏架构中
    /// </summary>
    /// <param name="architecture">要安装模块的目标游戏架构实例</param>
    public override void Install(IArchitecture architecture)
    {
        // 注册单位映射器实用程序到架构中
        // architecture.RegisterUtility(new UnitMapper());
    }
}


