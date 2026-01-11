using CosmicMiningCompany.scripts.setting;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.Abstractions.command;
using GFramework.Core.command;
using GFramework.Core.extensions;

namespace CosmicMiningCompany.scripts.command.graphics;

/// <summary>
/// 切换全屏模式命令类
/// </summary>
/// <param name="input">切换全屏命令输入参数</param>
public sealed class ToggleFullscreenCommand(ToggleFullscreenCommandInput input)
    : AbstractCommand<ToggleFullscreenCommandInput>(input)
{
    /// <summary>
    /// 执行切换全屏命令
    /// </summary>
    /// <param name="input">切换全屏命令输入参数</param>
    protected override async void OnExecute(ToggleFullscreenCommandInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        model.Graphics.Fullscreen = input.Fullscreen;

        await this.GetSystem<ISettingsSystem>()!.ApplyGraphics();
    }
}

/// <summary>
/// 切换全屏命令输入类
/// </summary>
public sealed class ToggleFullscreenCommandInput: ICommandInput
{
    /// <summary>
    /// 全屏状态属性
    /// </summary>
    public bool Fullscreen { get; set; }
}
