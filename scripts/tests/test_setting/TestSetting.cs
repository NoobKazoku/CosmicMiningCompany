using CosmicMiningCompany.scripts.setting;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scripts.tests.test_setting;

[ContextAware]
[Log]
public partial class TestSetting : Node, IController
{
    // 音频控件
    private HSlider MasterVolumeSlider => GetNode<HSlider>("%MasterVolumeSlider");
    private Label MasterVolumeValue => GetNode<Label>("%MasterVolumeValue");
    private HSlider BgmVolumeSlider => GetNode<HSlider>("%BgmVolumeSlider");
    private Label BgmVolumeValue => GetNode<Label>("%BgmVolumeValue");
    private HSlider SfxVolumeSlider => GetNode<HSlider>("%SfxVolumeSlider");
    private Label SfxVolumeValue => GetNode<Label>("%SfxVolumeValue");

    // 图形控件
    private CheckBox FullscreenCheckBox => GetNode<CheckBox>("%FullscreenCheckBox");
    private OptionButton ResolutionOptionButton => GetNode<OptionButton>("%ResolutionOptionButton");

    // 按钮
    private Button SaveButton => GetNode<Button>("%SaveButton");
    private Button LoadButton => GetNode<Button>("%LoadButton");
    private Button ResetButton => GetNode<Button>("%ResetButton");
    private Button DebugButton => GetNode<Button>("%DebugButton");

    private ISettingsModel _settingsModel = null!;
    private ISettingsSystem _settingsSystem = null!;
    private ISettingsStorageUtility _settingsStorageUtility = null!;

    // 分辨率选项
    private readonly Vector2I[] _resolutions =
    [
        new(1920, 1080),
        new(1366, 768),
        new(1280, 720),
        new(1024, 768)
    ];

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _settingsModel = this.GetModel<ISettingsModel>()!;
        _settingsSystem = this.GetSystem<ISettingsSystem>()!;
        _settingsStorageUtility = this.GetUtility<ISettingsStorageUtility>()!;

        InitializeUi();
        SetupEventHandlers();
        LoadSettings();
    }

    /// <summary>
    /// 初始化UI控件
    /// </summary>
    private void InitializeUi()
    {
        // 初始化分辨率选项
        ResolutionOptionButton.Clear();
        foreach (var resolution in _resolutions)
        {
            ResolutionOptionButton.AddItem($"{resolution.X}x{resolution.Y}");
        }
    }

    /// <summary>
    /// 设置事件处理器
    /// </summary>
    private void SetupEventHandlers()
    {
        // 音频滑块事件
        MasterVolumeSlider.ValueChanged += OnMasterVolumeChanged;
        BgmVolumeSlider.ValueChanged += OnBgmVolumeChanged;
        SfxVolumeSlider.ValueChanged += OnSfxVolumeChanged;

        // 图形控件事件
        FullscreenCheckBox.Toggled += OnFullscreenToggled;
        ResolutionOptionButton.ItemSelected += OnResolutionChanged;

        // 按钮事件
        SaveButton.Pressed += OnSaveSettings;
        LoadButton.Pressed += OnLoadSettings;
        ResetButton.Pressed += OnResetSettings;
        DebugButton.Pressed += OnDebugInfo;
    }

    /// <summary>
    /// 加载设置到UI
    /// </summary>
    private void LoadSettings()
    {
        var audio = _settingsModel.Audio;
        var graphics = _settingsModel.Graphics;

        // 设置音频控件
        MasterVolumeSlider.Value = audio.MasterVolume;
        MasterVolumeValue.Text = $"{Mathf.RoundToInt(audio.MasterVolume * 100)}%";
        
        BgmVolumeSlider.Value = audio.BgmVolume;
        BgmVolumeValue.Text = $"{Mathf.RoundToInt(audio.BgmVolume * 100)}%";
        
        SfxVolumeSlider.Value = audio.SfxVolume;
        SfxVolumeValue.Text = $"{Mathf.RoundToInt(audio.SfxVolume * 100)}%";

        // 设置图形控件
        FullscreenCheckBox.ButtonPressed = graphics.Fullscreen;

        // 设置分辨率选项
        var currentResolution = new Vector2I(graphics.ResolutionWidth, graphics.ResolutionHeight);
        for (var i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i] != currentResolution) continue;
            ResolutionOptionButton.Select(i);
            break;
        }
    }

    /// <summary>
    /// 主音量改变事件
    /// </summary>
    /// <param name="value">新的音量值</param>
    private void OnMasterVolumeChanged(double value)
    {
        MasterVolumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
        
        // 直接修改设置模型
        _settingsModel.Audio.MasterVolume = (float)value;
        _settingsSystem.ApplyAudio();
        
        _log.Debug($"主音量更改为: {value}");
    }

    /// <summary>
    /// BGM音量改变事件
    /// </summary>
    /// <param name="value">新的音量值</param>
    private void OnBgmVolumeChanged(double value)
    {
        BgmVolumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
        
        // 直接修改设置模型
        _settingsModel.Audio.BgmVolume = (float)value;
        _settingsSystem.ApplyAudio();
        
        _log.Debug($"BGM音量更改为: {value}");
    }

    /// <summary>
    /// 音效音量改变事件
    /// </summary>
    /// <param name="value">新的音量值</param>
    private void OnSfxVolumeChanged(double value)
    {
        SfxVolumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
        
        // 直接修改设置模型
        _settingsModel.Audio.SfxVolume = (float)value;
        _settingsSystem.ApplyAudio();
        
        _log.Debug($"音效音量更改为: {value}");
    }

    /// <summary>
    /// 全屏模式切换事件
    /// </summary>
    /// <param name="pressed">是否启用全屏</param>
    private void OnFullscreenToggled(bool pressed)
    {
        // 直接修改设置模型
        _settingsModel.Graphics.Fullscreen = pressed;
        _settingsSystem.ApplyGraphics();
        
        _log.Debug($"全屏模式切换为: {pressed}");
    }

    /// <summary>
    /// 分辨率改变事件
    /// </summary>
    /// <param name="index">选择的分辨率索引</param>
    private void OnResolutionChanged(long index)
    {
        var resolution = _resolutions[index];
        
        // 直接修改设置模型
        _settingsModel.Graphics.ResolutionWidth = resolution.X;
        _settingsModel.Graphics.ResolutionHeight = resolution.Y;
        _settingsSystem.ApplyGraphics();
        
        _log.Debug($"分辨率更改为: {resolution.X}x{resolution.Y}");
    }

    /// <summary>
    /// 保存设置事件
    /// </summary>
    private void OnSaveSettings()
    {
        _log.Debug("开始保存设置");
        
        try
        {
            // 创建设置数据对象
            var settingsData = new SettingsData
            {
                Audio = new AudioSettings
                {
                    MasterVolume = _settingsModel.Audio.MasterVolume,
                    BgmVolume = _settingsModel.Audio.BgmVolume,
                    SfxVolume = _settingsModel.Audio.SfxVolume
                },
                Graphics = new GraphicsSettings
                {
                    Fullscreen = _settingsModel.Graphics.Fullscreen,
                    ResolutionWidth = _settingsModel.Graphics.ResolutionWidth,
                    ResolutionHeight = _settingsModel.Graphics.ResolutionHeight
                }
            };
            
            // 保存到存储
            _settingsStorageUtility.Save(settingsData);
            _log.Info("设置已保存");
        }
        catch (System.Exception ex)
        {
            _log.Error("保存设置时发生错误: " + ex.Message);
        }
    }

    /// <summary>
    /// 加载设置事件
    /// </summary>
    private void OnLoadSettings()
    {
        _log.Debug("开始加载设置");
        
        try
        {
            // 从存储加载设置数据
            var settingsData = _settingsStorageUtility.Load();
            
            // 应用到当前设置模型
            ApplySettingsData(settingsData);
            
            // 更新UI
            LoadSettings();
            
            // 应用所有设置
            _settingsSystem.ApplyAll();
            _log.Info("设置已加载");
        }
        catch (System.Exception ex)
        {
            _log.Error("加载设置时发生错误: " + ex.Message);
        }
    }

    /// <summary>
    /// 应用设置数据到设置模型
    /// </summary>
    /// <param name="settingsData">设置数据</param>
    private void ApplySettingsData(SettingsData settingsData)
    {
        // 修改当前设置模型的值
        _settingsModel.Audio.MasterVolume = settingsData.Audio.MasterVolume;
        _settingsModel.Audio.BgmVolume = settingsData.Audio.BgmVolume;
        _settingsModel.Audio.SfxVolume = settingsData.Audio.SfxVolume;
        
        _settingsModel.Graphics.Fullscreen = settingsData.Graphics.Fullscreen;
        _settingsModel.Graphics.ResolutionWidth = settingsData.Graphics.ResolutionWidth;
        _settingsModel.Graphics.ResolutionHeight = settingsData.Graphics.ResolutionHeight;
    }

    /// <summary>
    /// 重置设置为默认值
    /// </summary>
    private void OnResetSettings()
    {
        _log.Debug("重置设置为默认值");
        
        try
        {
            // 重置音频设置
            MasterVolumeSlider.Value = 1.0f;
            BgmVolumeSlider.Value = 0.8f;
            SfxVolumeSlider.Value = 0.8f;
            
            // 重置图形设置
            FullscreenCheckBox.ButtonPressed = true;
            ResolutionOptionButton.Select(0); // 1920x1080
            
            // 立即应用更改
            _settingsSystem.ApplyAll();
            
            _log.Info("设置已重置为默认值");
        }
        catch (System.Exception ex)
        {
            _log.Error("重置设置时发生错误: " + ex.Message);
        }
    }

    /// <summary>
    /// 显示调试信息
    /// </summary>
    private void OnDebugInfo()
    {
        var audio = _settingsModel.Audio;
        var graphics = _settingsModel.Graphics;
        
        _log.Debug("=== 当前设置信息 ===");
        _log.Debug($"音频设置:");
        _log.Debug($"  主音量: {audio.MasterVolume:F2}");
        _log.Debug($"  BGM音量: {audio.BgmVolume:F2}");
        _log.Debug($"  音效音量: {audio.SfxVolume:F2}");
        
        _log.Debug($"图形设置:");
        _log.Debug($"  全屏模式: {graphics.Fullscreen}");
        _log.Debug($"  分辨率: {graphics.ResolutionWidth}x{graphics.ResolutionHeight}");
        _log.Debug("=====================");
    }
}