using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Hot :HBoxContainer,IController
{
	private TextureProgressBar ProgressBar => GetNode<TextureProgressBar>("%ProgressBar");
	private Label HotValue => GetNode<Label>("%HotValue");
	private SpaceShip SpaceShip => GetTree().Root.GetNode<SpaceShip>("Space/SpaceShip");
	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
		UpdateHotUI(); // 实时更新过热UI
	}

	private void UpdateHotUI()
	{	
		HotValue.Text = SpaceShip.Gun.Heat.ToString("F0");
		ProgressBar.Value = SpaceShip.Gun.Heat;
	}
}


