using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Fuel :HBoxContainer,IController
{
	private TextureProgressBar ProgressBar => GetNode<TextureProgressBar>("%ProgressBar");
	private Label FuelValue => GetNode<Label>("%FuelValue");
	private SpaceShip SpaceShip => GetTree().Root.GetNode<SpaceShip>("Space/SpaceShip");

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		UpdateFuelUI(); // 使用SpaceShip的当前Fuel值更新UI
		ProgressBar.MaxValue = SpaceShip.MaxFuel;
	}

	public override void _Process(double delta)
	{
		UpdateFuelUI(); // 实时更新燃料UI
	}

	private void UpdateFuelUI()
	{	
		FuelValue.Text = SpaceShip.Fuel.ToString("F0");
		ProgressBar.Value = SpaceShip.Fuel;
	}

	
}


