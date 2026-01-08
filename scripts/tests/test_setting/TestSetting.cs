using System;
using CosmicMiningCompany.scripts.command.audio;
using CosmicMiningCompany.scripts.command.graphics;
using CosmicMiningCompany.scripts.command.setting;
using CosmicMiningCompany.scripts.events.settings;
using CosmicMiningCompany.scripts.query;
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.command;
using GFramework.Core.extensions;
using GFramework.Core.query;
using GFramework.Godot.extensions;
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
	private Button BgmPlayButton => GetNode<Button>("%BgmPlayButton");
	private AudioStreamPlayer BgmPlayer => GetNode<AudioStreamPlayer>("%BgmPlayer");
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
		_settingsStorageUtility = this.GetUtility<ISettingsStorageUtility>()!;

		InitializeUi();
		SetupEventHandlers();
		this.RegisterEvent<SettingsChangedEvent>(_=>LoadSettings())
			.UnRegisterWhenNodeExitTree(this);
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
		BgmPlayButton.Pressed += ()=>BgmPlayer.Play();
	}

	/// <summary>
	/// 加载设置到UI
	/// </summary>
	private void LoadSettings()
	{
		var view = this.SendQuery(new GetCurrentSettingsQuery(new EmptyQueryInput()));
		MasterVolumeSlider.Value = view.MasterVolume;
		MasterVolumeValue.Text = $"{Mathf.RoundToInt(view.MasterVolume * 100)}%";

		BgmVolumeSlider.Value = view.BgmVolume;
		BgmVolumeValue.Text = $"{Mathf.RoundToInt(view.BgmVolume * 100)}%";

		SfxVolumeSlider.Value = view.SfxVolume;
		SfxVolumeValue.Text = $"{Mathf.RoundToInt(view.SfxVolume * 100)}%";
		FullscreenCheckBox.ButtonPressed = view.Fullscreen;

		var currentResolution = new Vector2I(view.ResolutionWidth, view.ResolutionHeight);
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
		
		this.SendCommand(new ChangeMasterVolumeCommand(new ChangeMasterVolumeCommandInput { Volume = (float)value }));
		
		_log.Debug($"主音量更改为: {value}");
	}

	/// <summary>
	/// BGM音量改变事件
	/// </summary>
	/// <param name="value">新的音量值</param>
	private void OnBgmVolumeChanged(double value)
	{
		BgmVolumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
		this.SendCommand(new ChangeBgmVolumeCommand(new ChangeBgmVolumeCommandInput { Volume = (float)value }));
		_log.Debug($"BGM音量更改为: {value}");
	}

	/// <summary>
	/// 音效音量改变事件
	/// </summary>
	/// <param name="value">新的音量值</param>
	private void OnSfxVolumeChanged(double value)
	{
		SfxVolumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
		this.SendCommand(new ChangeSfxVolumeCommand(new ChangeSfxVolumeCommandInput { Volume = (float)value }));
		_log.Debug($"音效音量更改为: {value}");
	}

	/// <summary>
	/// 全屏模式切换事件
	/// </summary>
	/// <param name="pressed">是否启用全屏</param>
	private void OnFullscreenToggled(bool pressed)
	{
		this.SendCommand(new ToggleFullscreenCommand(new ToggleFullscreenCommandInput { Fullscreen = pressed }));
		_log.Debug($"全屏模式切换为: {pressed}");
	}

	/// <summary>
	/// 分辨率改变事件
	/// </summary>
	/// <param name="index">选择的分辨率索引</param>
	private void OnResolutionChanged(long index)
	{
		var resolution = _resolutions[index];
		this.SendCommand(new ChangeResolutionCommand(new ChangeResolutionCommandInput { Width = resolution.X, Height = resolution.Y }));
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
			this.SendCommand(new SaveSettingsCommand(new EmptyCommentInput()));
			_log.Info("设置已保存");
		}
		catch (Exception ex)
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
			var data = _settingsStorageUtility.Load();
			this.SendCommand(new ApplySettingsDataCommand(new ApplySettingsDataCommandInput
			{
				Settings = data
			}));
			_log.Info("设置已加载");
		}
		catch (Exception ex)
		{
			_log.Error("加载设置时发生错误: " + ex.Message);
		}
	}

	/// <summary>
	/// 重置设置为默认值
	/// </summary>
	private void OnResetSettings()
	{
		_log.Debug("重置设置为默认值");
		
		try
		{
			this.SendCommand(new ResetSettingsCommand(new EmptyCommentInput()));
			_log.Info("设置已重置为默认值");
		}
		catch (Exception ex)
		{
			_log.Error("重置设置时发生错误: " + ex.Message);
		}
	}

	/// <summary>
	/// 显示调试信息
	/// </summary>
	private void OnDebugInfo()
	{
		var info = this.SendQuery(new GetSettingsDebugInfoQuery(new EmptyQueryInput()));
		_log.Debug(info.Text);
	}
}
