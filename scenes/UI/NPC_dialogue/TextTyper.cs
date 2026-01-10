using Godot;
using System;

public partial class TextTyper : RichTextLabel
{
    // 打字机文本->想要显示的实际文本
    public string _typerText = "欢迎回来，矿工！Rock and Stone!";

    // 打字机进度
    private int _progressIndex = 0;

    // 打字的速度
    private float _speed = 0.0075f;
    private float _timer = 0.0f; // 计时器


    public override void _Ready()
    {
        // 保留原始的空实现
    }

    public override void _Process(double delta)
    {
        // 判断是否完成打字
        if (_progressIndex >= _typerText.Length)
        {
            // 打字已完成，不执行任何操作
        }
        else
        {
            // 未完成打字逻辑
            if (_timer <= 0.0f)
            {
                _timer = _speed;
                PrintChar();
                _progressIndex++;
            }
            else
            {
                _timer -= (float)delta;
            }
        }
    }

    // 实现打字逻辑
    private void PrintChar()
    {
        char soloChar = _typerText[_progressIndex];
        // 当前实际显示的文本
        this.Text += soloChar.ToString();
    }

    // 公共方法用于设置要显示的文本
    public void SetTyperText(string text)
    {
        _typerText = text;
        _progressIndex = 0;
        this.Text = "";
        _timer = 0.0f;
    }

    // 检查打字是否完成
    public new bool IsFinished()
    {
        return _progressIndex >= _typerText.Length;
    }
}