using CosmicMiningCompany.scenes.component;
using CosmicMiningCompany.scripts.command.audio;
using CosmicMiningCompany.scripts.command.graphics;
using CosmicMiningCompany.scripts.command.setting;
using CosmicMiningCompany.scripts.query;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.command;
using GFramework.Core.extensions;
using GFramework.Core.query;
using GFramework.Godot.extensions.signal;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.Options;

/// <summary>
/// 选项设置界面控制器
/// 负责处理游戏设置界面的UI逻辑，包括音量控制、分辨率和全屏模式设置
/// </summary>
[ContextAware]
[Log]
public partial class Options : Control, IController
{
	/// <summary>
	/// 主音量控制容器
	/// </summary>
	private SettingVolumeContainer MasterVolume => GetNode<SettingVolumeContainer>("%MasterVolumeContainer");

	/// <summary>
	/// 背景音乐音量控制容器
	/// </summary>
	private SettingVolumeContainer BgmVolume => GetNode<SettingVolumeContainer>("%BgmVolumeContainer");

	/// <summary>
	/// 音效音量控制容器
	/// </summary>
	private SettingVolumeContainer SfxVolume => GetNode<SettingVolumeContainer>("%SfxVolumeContainer");

	/// <summary>
	/// 分辨率选择按钮
	/// </summary>
	private OptionButton ResolutionOptionButton => GetNode<OptionButton>("%ResolutionOptionButton");

	/// <summary>
	/// 全屏模式选择按钮
	/// </summary>
	private OptionButton FullscreenOptionButton => GetNode<OptionButton>("%FullscreenOptionButton");
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
		GetNode<Button>("%Back").Pressed += () =>
		{
			this.SendCommand(new SaveSettingsCommand(new EmptyCommentInput()));
			_log.Info("设置已保存");
			GetParent()?.RemoveChild(this);
			QueueFree();
		};
		InitializeUi();
		SetupEventHandlers();
	}

	/// <summary>
	/// 初始化用户界面
	/// 设置音量控制组件和分辨率选项的初始值
	/// </summary>
	private void InitializeUi()
	{
		var view = this.SendQuery(new GetCurrentSettingsQuery(new EmptyQueryInput()));
		MasterVolume.Initialize("主音量", view.MasterVolume);
		BgmVolume.Initialize("音乐音量", view.BgmVolume);
		SfxVolume.Initialize("音效音量", view.SfxVolume);
		// 初始化分辨率选项
		ResolutionOptionButton.Clear();
		foreach (var resolution in _resolutions)
		{
			ResolutionOptionButton.AddItem($"{resolution.X}x{resolution.Y}");
		}

		// 初始化全屏选项
		FullscreenOptionButton.Clear();
		FullscreenOptionButton.AddItem("全屏");
		FullscreenOptionButton.AddItem("窗口化");
		ResolutionOptionButton.Disabled = view.Fullscreen;
		FullscreenOptionButton.Selected = view.Fullscreen ? 0 : 1;
		var currentResolution = new Vector2I(view.ResolutionWidth, view.ResolutionHeight);
		for (var i = 0; i < _resolutions.Length; i++)
		{
			if (_resolutions[i] != currentResolution) continue;
			ResolutionOptionButton.Select(i);
			break;
		}
	}

	/// <summary>
	/// 设置事件处理器
	/// 为音量控制、分辨率和全屏模式选择器绑定事件处理逻辑
	/// </summary>
	private void SetupEventHandlers()
	{
		var signalName = SettingVolumeContainer.SignalName.VolumeChanged;
		MasterVolume
			.Signal(signalName)
			.To(Callable.From<float>(v =>
				this.SendCommand(new ChangeMasterVolumeCommand(
					new ChangeMasterVolumeCommandInput { Volume = v }))))
			.End();
		BgmVolume
			.Signal(signalName)
			.To(Callable.From<float>(v =>
				this.SendCommand(new ChangeBgmVolumeCommand(
					new ChangeBgmVolumeCommandInput { Volume = v }))))
			.End();
		SfxVolume
			.Signal(signalName)
			.To(Callable.From<float>(v =>
				this.SendCommand(new ChangeSfxVolumeCommand(
					new ChangeSfxVolumeCommandInput { Volume = v }))))
			.End();
		ResolutionOptionButton.ItemSelected += OnResolutionChanged;
		FullscreenOptionButton.ItemSelected += OnFullscreenChanged;
	}

	/// <summary>
	/// 分辨率改变事件
	/// </summary>
	/// <param name="index">选择的分辨率索引</param>
	private void OnResolutionChanged(long index)
	{
		var resolution = _resolutions[index];
		this.SendCommand(new ChangeResolutionCommand(new ChangeResolutionCommandInput
			{ Width = resolution.X, Height = resolution.Y }));
		
		// 显示调试信息
		GD.Print($"分辨率已保存为: {resolution.X}x{resolution.Y}");
	}

	/// <summary>
	/// 全屏模式改变事件
	/// </summary>
	/// <param name="index">选择的全屏模式索引</param>
	private void OnFullscreenChanged(long index)
	{
		var fullscreen = index == 0;
		this.SendCommand(new ToggleFullscreenCommand(new ToggleFullscreenCommandInput { Fullscreen = fullscreen }));
		
		// 禁用 / 启用分辨率选择
		ResolutionOptionButton.Disabled = fullscreen;
		
		// 显示调试信息
		GD.Print($"全屏模式已保存为: {fullscreen}");
	}
}
