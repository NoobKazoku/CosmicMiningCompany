using System.Collections.Generic;
using System.Linq;
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
    
    private readonly List<AudioStreamPlayer> _sfxPlayers = new();

    private int _sfxIndex;
    [Export]
    public int MaxSfxPlayerCount = 12;
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
    /// 舰船开火音效 - 存储舰船武器发射时的音效
    /// </summary>
    [Export] 
    public AudioStream ShipFireSfx { get; set; } = null!;
    
    /// <summary>
    /// 爆炸音效 - 存储爆炸效果的音效
    /// </summary>
    [Export] 
    public AudioStream ExplosionSfx { get; set; } = null!;
    
    /// <summary>
    /// UI点击音效 - 存储用户界面交互时的音效
    /// </summary>
    [Export] 
    public AudioStream UiClickSfx { get; set; } = null!;

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        BgmAudioStreamPlayer.Bus = "BGM";
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
        // 注册音效播放事件监听器
        this.RegisterEvent<PlaySfxEvent>(OnPlaySfx)
            .UnRegisterWhenNodeExitTree(this);
    }
    
    /// <summary>
    /// 创建新的音效播放器
    /// </summary>
    /// <returns>创建的音效播放器实例</returns>
    private AudioStreamPlayer CreateSfxPlayer()
    {
        var player = new AudioStreamPlayer
        {
            Bus = "SFX"
        };

        AddChild(player);
        _sfxPlayers.Add(player);

        return player;
    }
    
    /// <summary>
    /// 获取可用的音效播放器
    /// </summary>
    /// <returns>可用的音效播放器实例，若无可用播放器则返回null</returns>
    private AudioStreamPlayer? GetAvailableSfxPlayer()
    {
        // 1️⃣ 优先找一个没在播放的
        var availablePlayer = _sfxPlayers.FirstOrDefault(player => !player.Playing);
        if (availablePlayer != null)
            return availablePlayer;
        
        // 2️⃣ 如果没找到，且还没到上限 → 新建
        return _sfxPlayers.Count < MaxSfxPlayerCount ? CreateSfxPlayer() :
            // 3️⃣ 已达上限 → 丢弃
            null;
    }

    /// <summary>
    /// 处理音效播放事件
    /// </summary>
    /// <param name="event">音效播放事件</param>
    private void OnPlaySfx(PlaySfxEvent @event)
    {
        var player = GetAvailableSfxPlayer();
        if (player == null)
            return; // 达到上限，直接丢弃

        player.Stop();

        player.Stream = @event.SfxType switch
        {
            SfxType.ShipFire => ShipFireSfx,
            SfxType.Explosion => ExplosionSfx,
            SfxType.UiClick => UiClickSfx,
            _ => null
        };

        if (player.Stream == null)
            return;

        player.PitchScale = (float)GD.RandRange(0.95f, 1.05f);
        player.Play();
    }

}
