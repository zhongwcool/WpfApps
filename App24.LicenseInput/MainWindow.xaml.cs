using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace App24.LicenseInput;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DisableInputMethod(Part1);
        DisableInputMethod(Part2);
        DisableInputMethod(Part3);
        DisableInputMethod(Part4);

        // 添加接受粘贴处理
        DataObject.AddPastingHandler(Part1, OnPaste);
        DataObject.AddPastingHandler(Part2, OnPaste);
        DataObject.AddPastingHandler(Part3, OnPaste);
        DataObject.AddPastingHandler(Part4, OnPaste);
    }

    private static void DisableInputMethod(TextBox textBox)
    {
        InputMethod.SetIsInputMethodEnabled(textBox, false);
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        var caretPosition = textBox.CaretIndex;
        ShiftText(Part1.Text + Part2.Text + Part3.Text + Part4.Text);
        textBox.CaretIndex = caretPosition;
    }

    private void ShiftText(string inputCode)
    {
        if (string.IsNullOrEmpty(inputCode)) return;
        // 小写转大写
        inputCode = inputCode.ToUpper();
        // 剔除非法字符
        inputCode = Regex.Replace(inputCode, "[^0-9A-Za-z]", "");

        Part1.Text = inputCode.Substring(0, Math.Min(4, inputCode.Length));
        Part1.CaretIndex = Part1.Text.Length; // 将光标移动到最后
        Part1.Focus();
        if (inputCode.Length > 4)
        {
            Part2.Text = inputCode.Substring(4, Math.Min(4, inputCode.Length - 4));
            Part2.CaretIndex = Part2.Text.Length;
            Part2.Focus();
        }
        else
            Part2.Text = "";

        if (inputCode.Length > 8)
        {
            Part3.Text = inputCode.Substring(8, Math.Min(4, inputCode.Length - 8));
            Part3.CaretIndex = Part3.Text.Length;
            Part3.Focus();
        }
        else
            Part3.Text = "";

        if (inputCode.Length > 12)
        {
            Part4.Text = inputCode.Substring(12, Math.Min(4, inputCode.Length - 12));
            Part4.CaretIndex = Part4.Text.Length;
            Part4.Focus();
        }
        else
            Part4.Text = "";
    }

    private void MoveFocusBasedOnTextLength(TextBox currentTextBox)
    {
        if (currentTextBox.Text.Length == 4)
        {
            MoveFocusToNextTextBox(currentTextBox);
        }
        else if (currentTextBox.Text.Length == 0)
        {
            MoveFocusToPreviousTextBox(currentTextBox);
        }
    }

    private void MoveFocusToNextTextBox(TextBox currentTextBox)
    {
        if (currentTextBox == Part1)
            Part2.Focus();
        else if (currentTextBox == Part2)
            Part3.Focus();
        else if (currentTextBox == Part3)
            Part4.Focus();
    }

    private void MoveFocusToPreviousTextBox(TextBox currentTextBox)
    {
        if (currentTextBox == Part2)
        {
            Part1.Focus();
            Part1.CaretIndex = Part1.Text.Length;
        }
        else if (currentTextBox == Part3)
        {
            Part2.Focus();
            Part2.CaretIndex = Part2.Text.Length;
        }
        else if (currentTextBox == Part4)
        {
            Part3.Focus();
            Part3.CaretIndex = Part3.Text.Length;
        }
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!IsTextAllowed(e.Text))
        {
            e.Handled = true;
        }
        else
        {
            if (sender is not TextBox textBox) return;
            var summary = Part1.Text + Part2.Text + Part3.Text + Part4.Text;
            //把输入的字符插入到sum中，将当前textbox的光标换算出sum对应的位置
            var caretIndex = textBox.CaretIndex;
            summary = textBox.Name switch
            {
                nameof(Part1) => summary.Insert(caretIndex, e.Text),
                nameof(Part2) => summary.Insert(caretIndex + 4 * 1, e.Text),
                nameof(Part3) => summary.Insert(caretIndex + 4 * 2, e.Text),
                nameof(Part4) => summary.Insert(caretIndex + 4 * 3, e.Text),
                _ => summary
            };
            ShiftText(summary);

            MoveFocusBasedOnTextLength(textBox);
            e.Handled = true;
        }
    }

    private static bool IsTextAllowed(string text)
    {
        var regex = new Regex("[^0-9A-Za-z]");
        return !regex.IsMatch(text);
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (e.Key == Key.Back && textBox.Text.Length == 0)
        {
            MoveFocusToPreviousTextBox(textBox);
        }
    }

    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (!e.DataObject.GetDataPresent(typeof(string))) return;
        var pasteString = e.DataObject.GetData(typeof(string)) as string;
        if (string.IsNullOrEmpty(pasteString)) return;
        // 分割字符串
        ShiftText(pasteString!);

        e.CancelCommand();
    }
}