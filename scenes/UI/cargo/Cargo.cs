using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Cargo :VBoxContainer,IController
{
	public int OreCount = 0; //散矿
	public int GemCount = 0; //宝石
	public int OreGet = 10;	//矿物获取量

private Label OreLabel => GetNode<Label>("%散矿数量");
private Label GemLabel => GetNode<Label>("%宝石1数量");

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		
	}

    public override void _PhysicsProcess(double delta)
    {
        //更新UI
        UpdateOreUI();
    }

	public void UpdateOreUI()
	{
        OreLabel.Text = OreCount.ToString();
        GemLabel.Text = GemCount.ToString();
	}



	/// <summary>
	/// 获得矿物名，对应计数+1
	/// </summary>
	/// <param name="OreName">矿物名</param>
	public void CollectOre(string OreName)
	{
		if (OreName == "散矿")
		{
			GD.Print("散矿+1");
			OreCount += OreGet;
		}
		if (OreName == "宝石1")
		{
			GD.Print("宝石1 +1");
			GemCount += OreGet;
		}
		
	}
}

