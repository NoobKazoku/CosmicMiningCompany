using CosmicMiningCompany.scripts.setting;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.command;
using GFramework.Core.extensions;

namespace CosmicMiningCompany.scripts.command.setting;

/// <summary>
/// 保存游戏设置命令类
/// 负责将当前游戏设置数据保存到存储中
/// </summary>
/// <param name="input">命令输入参数，类型为EmptyCommentInput</param>
public sealed class SaveSettingsCommand(EmptyCommentInput input) : AbstractCommand<EmptyCommentInput>(input)
{
    /// <summary>
    /// 执行保存设置命令的主逻辑
    /// </summary>
    /// <param name="input">命令输入参数</param>
    protected override void OnExecute(EmptyCommentInput input)
    {
        // 获取设置模型和存储工具实例
        var model = this.GetModel<ISettingsModel>()!;
        var storage = this.GetUtility<ISettingsStorageUtility>()!;

        // 构建设置数据对象，包含音频和图形设置
        var data = new SettingsData
        {
            Audio = new AudioSettings
            {
                MasterVolume = model.Audio.MasterVolume,
                BgmVolume = model.Audio.BgmVolume,
                SfxVolume = model.Audio.SfxVolume
            },
            Graphics = new GraphicsSettings
            {
                Fullscreen = model.Graphics.Fullscreen,
                ResolutionWidth = model.Graphics.ResolutionWidth,
                ResolutionHeight = model.Graphics.ResolutionHeight
            }
        };

        // 将设置数据保存到存储中
        storage.Save(data);
    }
}
