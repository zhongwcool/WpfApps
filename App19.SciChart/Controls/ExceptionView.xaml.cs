using System;
using System.Windows;
using Mar.Cheese;
using SciChart.Charting.Visuals;

namespace App19.Controls;

public partial class ExceptionView : Window
{
    private readonly Exception? _exception;

    public ExceptionView(Exception? exception)
    {
        InitializeComponent();
        _exception = exception;
        ExceptionUtil.TaskGetText(exception).ContinueWith(task => { exceptionViewer.Text = task.Result; });
    }

    private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetData(DataFormats.Text, FormatEmail());
    }

    private void EmailSupport_Click(object sender, RoutedEventArgs e)
    {
        EmailUtil.SendEmail(
            "2872700763@qq.com",
            "zhongwcool@qq.com",
            $"崩溃日志-{_exception?.Message}",
            Uri.EscapeDataString(FormatEmail()),
            "smtp.qq.com", 587, "2872700763", "utyydjctjirrdfgc");
    }

    private string FormatEmail()
    {
        return $"Dear Support, \r\n\r\nI was running the SciChart {SciChartSurface.VersionInfo} examples and saw " +
               $"this Unhandled Exception. \r\n\r\nCan you help? \r\n\r\nThank you!\r\n\r\n\r\n{exceptionViewer.Text}";
    }
}