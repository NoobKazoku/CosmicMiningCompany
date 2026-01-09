using Godot;
using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;


[ContextAware]
[Log]
public partial class Cargo :VBoxContainer,IController
{
	public int OreGet = 10;	//矿物获取量

private Label OreLabel => GetNode<Label>("%散矿数量");
private Label GemLabel => GetNode<Label>("%宝石1数量");

	/// <summary>
	/// 节点准备就绪时的回调方法
	/// 在节点添加到场景树后调用
	/// </summary>
	public override void _Ready()
	{
		OreGet = PlayerManager.Instance.OreGet;
		//重置任务矿石数量
		PlayerManager.Instance.OreCount = 0;
		PlayerManager.Instance.GemCount = 0;
	}

    public override void _Process(double delta)
    {
        //更新UI
        UpdateOreUI();
    }

	public void UpdateOreUI()
	{
        OreLabel.Text = PlayerManager.Instance.OreCount.ToString();
        GemLabel.Text = PlayerManager.Instance.GemCount.ToString();
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
			PlayerManager.Instance.OreCount += OreGet;

		}
		if (OreName == "宝石1")
		{
			GD.Print("宝石1 +1");
			PlayerManager.Instance.GemCount += OreGet;

		}
		
	}
}

