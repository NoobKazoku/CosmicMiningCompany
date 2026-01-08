using System;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scenes.component;

/// <summary>
/// 设置音量容器组件，用于显示和控制音量滑块
/// </summary>
[ContextAware]
[Log]
public partial class SettingVolumeContainer :HBoxContainer,IController
{
	/// <summary>
	/// 获取音量滑块控件
	/// </summary>
	private HSlider SettingVolumeSlider => GetNode<HSlider>("%SettingVolumeSlider");
	
	/// <summary>
	/// 获取音量标签控件
	/// </summary>
	private Label SettingVolumeLabel => GetNode<Label>("%SettingVolumeLabel");
	
	/// <summary>
	/// 获取音量值显示标签控件
	/// </summary>
	private Label SettingVolumeValue => GetNode<Label>("%SettingVolumeValue");
	
	[Signal]
	public delegate void VolumeChangedEventHandler(float value);
	
	/// <summary>
	/// 组件准备就绪时的初始化方法
	/// </summary>
	public override void _Ready()
	{
		// 订阅滑块值改变事件
		SettingVolumeSlider.ValueChanged += OnSliderValueChanged;
	}
	
	/// <summary>
	/// 初始化组件
	/// </summary>
	/// <param name="title">音量设置的标题文本</param>
	/// <param name="initialValue">初始音量值</param>
	public void Initialize(string title, float initialValue)
	{
		SettingVolumeLabel.Text = title;
		SetValue(initialValue);
	}

	/// <summary>
	/// 外部设置音量值（不会触发事件）
	/// </summary>
	/// <param name="value">要设置的音量值</param>
	public void SetValue(float value)
	{
		SettingVolumeSlider.Value = value;
		UpdateValueText(value);
	}

	/// <summary>
	/// 滑块值改变时的回调方法
	/// </summary>
	/// <param name="value">滑块的新值</param>
	private void OnSliderValueChanged(double value)
	{
		var v = (float)value;
		UpdateValueText(v);
		EmitSignalVolumeChanged(v);
	}

	/// <summary>
	/// 更新音量值显示文本
	/// </summary>
	/// <param name="value">当前音量值</param>
	private void UpdateValueText(float value)
	{
		SettingVolumeValue.Text = $"{Mathf.RoundToInt(value * 100)}%";
	}
}
