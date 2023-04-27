using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using App16.Python.Control;
using Microsoft.Win32;

namespace App16.Python.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Prepare();
        CheckVenv();
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

    private void Test(string arguments)
    {
        if (string.IsNullOrEmpty(_lastRelativePath))
        {
            Dispatcher.Invoke(() => { Block.Text = "未指定python文件"; });
            return;
        }

        var start = DateTime.Now;

        var startInfo = new ProcessStartInfo
        {
            FileName = "venv/Scripts/python.exe", // Python解释器路径
            Arguments = $"{_lastRelativePath} {arguments}", // script 和 参数
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true, // 设置不创建进程窗口
            WindowStyle = ProcessWindowStyle.Hidden // 隐藏进程窗口
        };
        Dispatcher.Invoke(() =>
        {
            Block.Text = "正在执行...";
            Title = $"{startInfo.FileName} {startInfo.Arguments}";
        });
        using var process = Process.Start(startInfo);
        using var reader = process?.StandardOutput;
        var result = reader?.ReadToEnd();
        Dispatcher.Invoke(() => { Block.Text = result; });

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
        var ts = DateTime.Now.ToLongTimeString();
        _serialTaskExecutor.AddTask(() => Test(ts));
    }

    private readonly ParallelTaskExecutor _parallelTaskExecutor = new();

    private void ButtonParallel_OnClick(object sender, RoutedEventArgs e)
    {
        var ts = DateTime.Now.ToLongTimeString();
        _parallelTaskExecutor.AddTask(Task.Run(() => Test(ts)));
    }

    private void Prepare()
    {
        // 从配置文件中读取上次选择的文件路径
        var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        if (configuration.AppSettings.Settings["LastFilePath"] != null)
        {
            _lastFilePath = configuration.AppSettings.Settings["LastFilePath"].Value;
            _lastRelativePath = System.IO.Path.GetRelativePath(Environment.CurrentDirectory, _lastFilePath); // 转换为相对路径
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
            InitialDirectory = System.IO.Path.GetDirectoryName(_lastFilePath),
            Title = "选择Python文件",
            Filter = "Python文件|*.py"
        };
        if (openFileDialog.ShowDialog() != true) return;
        // 用户点击了“打开”按钮
        _lastFilePath = openFileDialog.FileName;
        _lastRelativePath = System.IO.Path.GetRelativePath(Environment.CurrentDirectory, _lastFilePath); // 转换为相对路径
        TxtPath.Text = _lastRelativePath;

        // 记录选择的文件路径到配置文件中
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["LastFilePath"].Value = _lastFilePath;
        config.Save();
    }
}