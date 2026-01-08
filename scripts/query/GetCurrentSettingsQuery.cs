using CosmicMiningCompany.scripts.query.input;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.extensions;
using GFramework.Core.query;

namespace CosmicMiningCompany.scripts.query;
    
/// <summary>
/// 获取当前设置的查询类
/// </summary>
/// <param name="input">空查询输入参数</param>
public sealed class GetCurrentSettingsQuery(EmptyQueryInput input) : AbstractQuery<EmptyQueryInput, SettingsView>(input)
{
    /// <summary>
    /// 执行获取当前设置的查询操作
    /// </summary>
    /// <param name="input">空查询输入参数</param>
    /// <returns>包含当前设置信息的SettingsView对象</returns>
    protected override SettingsView OnDo(EmptyQueryInput input)
    {
        // 从模型中获取设置数据
        var model = this.GetModel<ISettingsModel>()!;

        // 构建并返回设置视图对象
        return new SettingsView
        {
            MasterVolume = model.Audio.MasterVolume,
            BgmVolume = model.Audio.BgmVolume,
            SfxVolume = model.Audio.SfxVolume,
            Fullscreen = model.Graphics.Fullscreen,
            ResolutionWidth = model.Graphics.ResolutionWidth,
            ResolutionHeight = model.Graphics.ResolutionHeight
        };
    }
}
