using Godot;
using System;

// public partial class NewScript : Node
// {
// }

public partial class JsonReader : Node
{
	public override void _Ready()
	{
		// 游戏启动时运行的代码
		GD.Print("JsonReader 已启动");

		// 示例：读取并打印 JSON 数据
		var data = ReadJsonToDictionary("res://assets/data/Asteroid.json");
		GD.Print($"读取的数据: {data}");
	}

	// 读取JSON文件并返回字典
	public Godot.Collections.Dictionary ReadJsonToDictionary(string filePath)
	{
		// 检查文件是否存在
		if (!FileAccess.FileExists(filePath))
		{
			GD.PrintErr($"文件不存在: {filePath}");
			return new Godot.Collections.Dictionary();
		}

		try
		{
			// 使用FileAccess读取文件内容
			using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
			string jsonContent = file.GetAsText();

			// 使用Json解析JSON字符串为字典
			var json = new Json();
			var parseResult = json.Parse(jsonContent);

			if (parseResult == Error.Ok)
			{
				// 获取解析后的数据并转换为字典
				Variant data = json.Data;
				return data.AsGodotDictionary();
			}
			else
			{
				GD.PrintErr($"JSON解析失败: {json.GetErrorMessage()}, 错误位置: {json.GetErrorLine()}");
				return new Godot.Collections.Dictionary();
			}
		}
		catch (System.Exception e)
		{
			GD.PrintErr($"读取文件时发生错误: {e.Message}");
			return new Godot.Collections.Dictionary();
		}
	}
}