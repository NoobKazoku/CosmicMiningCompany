using CosmicMiningCompany.scripts.setting;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.Abstractions.command;
using GFramework.Core.command;
using GFramework.Core.extensions;

namespace CosmicMiningCompany.scripts.command.graphics;

/// <summary>
/// 更改分辨率命令类，用于处理分辨率更改操作
/// </summary>
/// <param name="input">分辨率更改命令输入参数</param>
public sealed class ChangeResolutionCommand(ChangeResolutionCommandInput input)
    : AbstractCommand<ChangeResolutionCommandInput>(input)
{
    /// <summary>
    /// 执行分辨率更改命令
    /// </summary>
    /// <param name="input">分辨率更改命令输入参数，包含新的宽度和高度值</param>
    protected override void OnExecute(ChangeResolutionCommandInput input)
    {
        var model = this.GetModel<ISettingsModel>()!;
        model.Graphics.ResolutionWidth = input.Width;
        model.Graphics.ResolutionHeight = input.Height;

        this.GetSystem<ISettingsSystem>()!.ApplyGraphics();
    }
}

/// <summary>
/// 分辨率更改命令输入类，用于传递分辨率更改所需的参数
/// </summary>
public sealed class ChangeResolutionCommandInput : ICommandInput
{
    /// <summary>
    /// 获取或设置分辨率的宽度
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// 获取或设置分辨率的高度
    /// </summary>
    public int Height { get; set; }
}
