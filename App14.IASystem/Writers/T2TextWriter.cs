#nullable enable
using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace App14.IASystem.Writers;

public class T2TextWriter : TextWriter
{
    private readonly TextBlock _outputTextBlock; // 替换为你的界面控件
    private const int MAX_LENGTH = 2000;

    public T2TextWriter(TextBlock textBlock)
    {
        _outputTextBlock = textBlock;
    }

    public override void Write(char value)
    {
        _outputTextBlock.Dispatcher.Invoke(() =>
        {
            var text = _outputTextBlock.Text;
            if (text.Length > MAX_LENGTH)
            {
                var index = text.IndexOf('\n');
                text = index != -1 ? text[(index + 1)..] : string.Empty;
                _outputTextBlock.Text = text;
            }

            _outputTextBlock.Text += value.ToString();
        });
    }

    public override void WriteLine(string? value)
    {
        _outputTextBlock.Dispatcher.Invoke(() =>
        {
            var text = _outputTextBlock.Text;
            if (text.Length > MAX_LENGTH)
            {
                var index = text.IndexOf('\n');
                text = index != -1 ? text[(index + 1)..] : string.Empty;
                _outputTextBlock.Text = text;
            }

            _outputTextBlock.Text += value + Environment.NewLine;
        });
    }

    public override Encoding Encoding => Encoding.UTF8;
}