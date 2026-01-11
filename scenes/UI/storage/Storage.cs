using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using CosmicMiningCompany.scripts.data.interfaces;


[ContextAware]
[Log]
public partial class Storage : Panel, IController
{
	private Label OreLabel => GetNode<Label>("%散矿数量");
	private Label GemLabel => GetNode<Label>("%宝石1数量");

	public override void _Process(double delta)
	{
		if (PlayerManager.Instance != null)
		{
			OreLabel.Text = PlayerManager.Instance.TotalOreCount.ToString();
			GemLabel.Text = PlayerManager.Instance.TotalGemCount.ToString();
		}
	}
}
