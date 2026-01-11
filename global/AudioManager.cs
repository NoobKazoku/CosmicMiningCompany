using CosmicMiningCompany.scripts.enums;
using CosmicMiningCompany.scripts.events.audio;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.Godot.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.global;

/// <summary>
/// 音频管理器类
/// 负责管理游戏中的背景音乐播放
/// </summary>
[ContextAware]
[Log]
public partial class AudioManager : Node, IController
{
    /// <summary>
    /// 获取背景音乐音频流播放器节点
    /// </summary>
    private AudioStreamPlayer BgmAudioStreamPlayer => GetNode<AudioStreamPlayer>("%BgmAudioStreamPlayer");

    /// <summary>
    /// 背景音乐音频流
    /// </summary>
    [Export]
    public AudioStream BgmAudioStream { get; set; } = null!;

    /// <summary>
    /// 游戏中音频流
    /// </summary>
    [Export]
    public AudioStream GamingAudioStream { get; set; } = null!;

    /// <summary>
    /// 准备中音频流
    /// </summary>
    [Export]
    public AudioStream ReadyAudioStream { get; set; } = null!;

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        // 注册背景音乐变更事件监听器
        this.RegisterEvent<BgmChangedEvent>(@event =>
        {
            // 停止当前播放的背景音乐
            BgmAudioStreamPlayer.Stop();

            // 根据事件中的背景音乐类型设置对应的音频流
            BgmAudioStreamPlayer.Stream = @event.BgmType switch
            {
                BgmType.Gaming => GamingAudioStream,
                BgmType.MainMenu => BgmAudioStream,
                BgmType.Ready => ReadyAudioStream,
                _ => null
            };

            // 如果音频流不为空则开始播放
            if (BgmAudioStreamPlayer.Stream is not null)
            {
                BgmAudioStreamPlayer.Play();
            }
        }).UnRegisterWhenNodeExitTree(this);
    }
}