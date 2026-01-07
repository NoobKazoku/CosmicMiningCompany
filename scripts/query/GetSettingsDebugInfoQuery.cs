using CosmicMiningCompany.scripts.query.input;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.extensions;
using GFramework.Core.query;

namespace CosmicMiningCompany.scripts.query;

/// <summary>
/// 获取设置调试信息查询类
/// 该查询用于获取当前应用的音频和图形设置信息，并以格式化的文本形式返回
/// </summary>
/// <param name="input">空查询输入参数</param>
public sealed class GetSettingsDebugInfoQuery(EmptyQueryInput input)
    : AbstractQuery<EmptyQueryInput, SettingsDebugInfo>(input)
{
    /// <summary>
    /// 执行查询操作，获取当前设置的调试信息
    /// </summary>
    /// <param name="input">空查询输入参数</param>
    /// <returns>包含格式化设置信息文本的SettingsDebugInfo对象</returns>
    protected override SettingsDebugInfo OnDo(EmptyQueryInput input)
    {
        // 获取设置模型实例
        var model = this.GetModel<ISettingsModel>()!;

        // 构建并返回包含音频和图形设置信息的调试信息对象
        return new SettingsDebugInfo
        {
            Text =
                $"""
                 === 当前设置信息 ===
                 音频设置:
                   主音量: {model.Audio.MasterVolume:F2}
                   BGM音量: {model.Audio.BgmVolume:F2}
                   音效音量: {model.Audio.SfxVolume:F2}

                 图形设置:
                   全屏模式: {model.Graphics.Fullscreen}
                   分辨率: {model.Graphics.ResolutionWidth}x{model.Graphics.ResolutionHeight}
                 =====================
                 """
        };
    }
}
