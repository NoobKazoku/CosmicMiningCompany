using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Cargo_2 :VBoxContainer,IController
{
	private Label OreLabel => GetNode<Label>("%散矿数量");
	private Label GemLabel => GetNode<Label>("%宝石1数量");
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
public override void _Ready()
	{
		// 开始动画，从0逐渐增加到PlayerManager中的count值
		StartCountAnimation();
	}
	
	/// <summary>
	/// 开始计数动画，从0逐渐增加到目标值
	/// </summary>
	private void StartCountAnimation()
	{
		// 保存当前目标值作为动画终点
		int targetOreCount = PlayerManager.Instance.OreCount;
		int targetGemCount = PlayerManager.Instance.GemCount;
		
		// 重置显示为0开始动画
		OreLabel.Text = "0";
		GemLabel.Text = "0";
		
		// 创建动画，从0到目标值，用时2秒
		var tween = CreateTween();
		tween.SetParallel(true); // 并行处理两个标签的动画
		
		// 动画 OreCount 从 0 到目标值
		tween.TweenMethod(
			new Callable(this, nameof(SetAnimatedOreCount)),
			0,
			targetOreCount,
			2.0f // 2秒完成动画
		).SetTrans(Tween.TransitionType.Linear);
		
		// 动画 GemCount 从 0 到目标值
		tween.TweenMethod(
			new Callable(this, nameof(SetAnimatedGemCount)),
			0,
			targetGemCount,
			2.0f // 2秒完成动画
		).SetTrans(Tween.TransitionType.Linear);
	}
	
	/// <summary>
	/// 设置动画中的 OreCount 值
	/// </summary>
	/// <param name="value">当前动画值</param>
	private void SetAnimatedOreCount(float value)
	{
		OreLabel.Text = ((int)value).ToString();
	}
	
	/// <summary>
	/// 设置动画中的 GemCount 值
	/// </summary>
	/// <param name="value">当前动画值</param>
	private void SetAnimatedGemCount(float value)
	{
		GemLabel.Text = ((int)value).ToString();
	}
}


