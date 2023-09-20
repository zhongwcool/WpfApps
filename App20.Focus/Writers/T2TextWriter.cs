using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace App20.Focus.Writers;

public class T2TextWriter : TextWriter
{
    private readonly TextBox _outputTextBox; // 替换为你的界面控件

    public T2TextWriter(TextBox textBox)
    {
        _outputTextBox = textBox;
        _outputTextBox.TextChanged += (_, _) =>
        {
            if (_outputTextBox.Text.Length <= _outputTextBox.MaxLength) return;
            var lines = _outputTextBox.Text.Split('\n').ToList();

            if (lines.Count <= 1) return;
            lines.RemoveAt(0);
            _outputTextBox.Text = string.Join("\n", lines);
        };
    }

    public override void Write(char value)
    {
        _outputTextBox.Dispatcher.Invoke(() => { _outputTextBox.AppendText(value.ToString()); });
    }

    public override void WriteLine(string? value)
    {
        _outputTextBox.Dispatcher.Invoke(() => { _outputTextBox.AppendText(value + Environment.NewLine); });
    }

    public override Encoding Encoding => Encoding.UTF8;
}