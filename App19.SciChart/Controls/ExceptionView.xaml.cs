using System;
using System.Windows;
using Mar.Console;
using SciChart.Charting.Visuals;

namespace App19.SciChart00.Controls;

public partial class ExceptionView : Window
{
    private readonly Exception? _exception;

    public ExceptionView(Exception? exception)
    {
        InitializeComponent();
        _exception = exception;
        LogException(exception);
    }

    private void LogException(Exception? exception)
    {
        if (exception == null) return;

        exceptionViewer.Text += exception.GetType().Name + ": " + exception.Message + Environment.NewLine;
        exceptionViewer.Text +=
            "-------------------------------------------" + Environment.NewLine + Environment.NewLine;
        exceptionViewer.Text += "Stack Trace: " + Environment.NewLine;
        exceptionViewer.Text += exception.StackTrace + Environment.NewLine + Environment.NewLine;

        LogException(exception.InnerException);
    }

    private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetData(DataFormats.Text, FormatEmail());
    }

    private void EmailSupport_Click(object sender, RoutedEventArgs e)
    {
        EmailUtil.SendEmail("2872700763@qq.com", "zhongwcool@qq.com", $"崩溃日志-{_exception?.Message}",
            Uri.EscapeDataString(FormatEmail()), "smtp.qq.com", 587, "2872700763", "utyydjctjirrdfgc");
    }

    private string FormatEmail()
    {
        return
            string.Format(
                "Dear Support, \r\n\r\nI was running the SciChart {0} examples and saw this Unhandled Exception. \r\n\r\nCan you help? \r\n\r\nThank you!\r\n\r\n\r\n{1}",
                SciChartSurface.VersionInfo, exceptionViewer.Text);
    }
}