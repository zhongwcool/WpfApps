using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using App16.Python.Control;
using App16.Python.Models;
using App16.Python.Utils;
using App16.Python.ViewModels;
using Mar.Controls.Tool;
using Microsoft.Win32;
using Serilog;

namespace App16.Python.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        _vm = new MainViewModel();
        DataContext = _vm;

        Task.Delay(500).ContinueWith(_ =>
        {
            Dispatcher.Invoke(() =>
            {
                var console = new ConsoleWindow();
                console.Show();
            });
        });

        Prepare();
        CheckVenv();
        SendTimer();
    }

    private readonly DispatcherTimer _dispatcherTimer = new();

    private void SendTimer()
    {
        _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
        _dispatcherTimer.Start();
    }

    private void CheckVenv()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "venv/Scripts/python.exe", // Python解释器路径
            Arguments = "-V", // script 和 参数
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true, // 设置不创建进程窗口
            WindowStyle = ProcessWindowStyle.Hidden // 隐藏进程窗口
        };
        try
        {
            using var process = Process.Start(startInfo);
            using var reader = process?.StandardOutput;
            var result = reader?.ReadToEnd();
            if (string.IsNullOrEmpty(result))
            {
                PanelOp.IsEnabled = false;
                TxtCost.Text = "未配置Python虚拟机";
            }
            else
            {
                PanelOp.IsEnabled = true;
                TxtCost.Text = "Python虚拟机已就绪";
            }
        }
        catch (Exception e)
        {
            PanelOp.IsEnabled = false;
            TxtCost.Text = "未配置Python虚拟机";
        }
    }

    private void Test(RenWu renWu)
    {
        if (string.IsNullOrEmpty(_lastRelativePath))
        {
            Log.Debug("未指定python文件");
            return;
        }

        var start = DateTime.Now;

        var startInfo = new ProcessStartInfo
        {
            FileName = "venv/Scripts/python.exe", // Python解释器路径
            Arguments = $"{_lastRelativePath} {renWu.Id} {renWu.Time} {renWu.Title}", // script 和 参数
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true, // 设置不创建进程窗口
            WindowStyle = ProcessWindowStyle.Hidden // 隐藏进程窗口
        };
        Log.Debug($"正在执行: {startInfo.FileName} {startInfo.Arguments}");
        using var process = Process.Start(startInfo);
        using var reader = process?.StandardOutput;
        var result = reader?.ReadToEnd();
        Dispatcher.Invoke(() =>
        {
            if (result != null) Log.Debug(result);
            Random random = new();
            var randomValue = random.Next(1, 100);
            _vm.AddData(randomValue);
        });

        var tspan = DateTime.Now - start; //求时间差
        Dispatcher.Invoke(() =>
        {
            var cost = (int)Math.Round(tspan.TotalMilliseconds, MidpointRounding.AwayFromZero); // 四舍五入取整
            TxtCost.Text = $"代码耗时：{cost}ms"; //获取代码段执行时间
        });
    }

    private void ButtonCheck_OnClick(object sender, RoutedEventArgs e)
    {
        CheckVenv();
    }

    private readonly SerialTaskExecutor _serialTaskExecutor = new();

    private void ButtonRun_OnClick(object sender, RoutedEventArgs e)
    {
        DoDemo(_serialTaskExecutor);
    }

    private readonly Serial2TaskExecutor _serial2TaskExecutor = new();

    private void ButtonRun02_OnClick(object sender, RoutedEventArgs e)
    {
        DoDemo(_serial2TaskExecutor);
    }

    private readonly ParallelTaskExecutor _parallelTaskExecutor = new();

    private void ButtonParallel_OnClick(object sender, RoutedEventArgs e)
    {
        DoDemo(_parallelTaskExecutor);
    }

    private void Prepare()
    {
        // 从配置文件中读取上次选择的文件路径
        var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        if (configuration.AppSettings.Settings["LastFilePath"] != null)
        {
            _lastFilePath = configuration.AppSettings.Settings["LastFilePath"].Value;
            _lastRelativePath = Path.GetRelativePath(Environment.CurrentDirectory, _lastFilePath); // 转换为相对路径
            TxtPath.Text = _lastRelativePath;
            return;
        }

        // 如果不存在，创建该键，并将初始值设置为空字符串
        configuration.AppSettings.Settings.Add("LastFilePath", "");
        configuration.Save(ConfigurationSaveMode.Modified);
    }

    private string _lastFilePath = "";
    private string _lastRelativePath = "";

    private void ButtonSelect_OnClick(object sender, RoutedEventArgs e)
    {
        // 弹出选择文件对话框，设置初始目录为上次选择的文件所在目录
        var openFileDialog = new OpenFileDialog()
        {
            InitialDirectory = Path.GetDirectoryName(_lastFilePath),
            Title = "选择Python文件",
            Filter = "Python文件|*.py"
        };
        if (openFileDialog.ShowDialog() != true) return;
        // 用户点击了“打开”按钮
        _lastFilePath = openFileDialog.FileName;
        _lastRelativePath = Path.GetRelativePath(Environment.CurrentDirectory, _lastFilePath); // 转换为相对路径
        TxtPath.Text = _lastRelativePath;

        // 记录选择的文件路径到配置文件中
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["LastFilePath"].Value = _lastFilePath;
        config.Save();
    }

    #region Dummy

    private void DoDemo(ITaskExecutor executor)
    {
        var model = JsonUtil.Load<RenWuModel>(JSON_FILE);
        if (null == model) return;

        foreach (var soul in model.Tasks) executor.AddTask(() => Test(soul));
    }

    private const string JSON_FILE = "tasks.json";

    #endregion

    private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
    {
        _dispatcherTimer.Tick -= _vm.Timer_Tick;
    }

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
    {
        _dispatcherTimer.Tick += _vm.Timer_Tick;
    }
}