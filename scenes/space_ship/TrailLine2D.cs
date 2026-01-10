using Godot;
using System.Collections.Generic;

public partial class TrailLine2D : Line2D
{
	[Export] public int MaxPoints { get; set; } = 10;
	[Export] public float PointInterval { get; set; } = 0.03f; // 添加点的时间间隔

	private float _accumulator = 0f;

	public override void _Ready()
	{
		// 初始化时添加第一个点
		AddPoint(ToLocal(GetParent<Node2D>().GlobalPosition));
	}

	public override void _Process(double delta)
	{
		_accumulator += (float)delta;

		// 只有经过指定时间间隔才添加新点
		if (_accumulator >= PointInterval)
		{
			UpdateTrail();
			_accumulator = 0f; // 重置累加器
		}
	}

	private void UpdateTrail()
	{
		// 添加新点到Line2D
		Vector2 parentPosition = GetParent<Node2D>().GlobalPosition;
		// Vector2 localPosition = ToLocal(parentPosition);
		// AddPoint(localPosition);
		AddPoint(parentPosition);

		// 限制点的数量
		while (GetPointCount() > MaxPoints)
		{
			RemovePoint(0);
		}
	}
}






