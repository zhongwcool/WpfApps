using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using App12.SQLite.HotelDbContext;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace App12.SQLite;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private readonly string _file = Path.Combine("00-Log", "tool.txt");

    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:l}{NewLine}{Exception}")
            .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:l}{NewLine}{Exception}")
            .WriteTo.File(_file,
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {Message:l}{NewLine}{Exception}")
            .CreateLogger();
        PrintSystemInfo();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using var dbContext = new HotelContext();
        // 检查数据模型是否发生了变化
        if (!dbContext.Database.GetPendingMigrations().Any()) return;
        // 应用未应用的迁移
        try
        {
            dbContext.Database.Migrate();
        }
        catch (Exception exception)
        {
            var task = TaskTextException(exception);
            task.ContinueWith(_ => { Log.Error("{TaskResult}", task.Result); });
        }
    }

    private static async Task<string> TaskTextException(Exception exception)
    {
        var stringBuilder = new StringBuilder();

        await Task.Run(() =>
        {
            while (true)
            {
                if (exception == null) break;

                stringBuilder.Append(exception.GetType().Name + ": " + exception.Message + Environment.NewLine);
                stringBuilder.Append("-------------------------------------------" + Environment.NewLine);
                stringBuilder.Append("Stack Trace: " + Environment.NewLine);
                stringBuilder.Append(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        });

        return stringBuilder.ToString();
    }

    #region 打印运行环境

    private static async void PrintSystemInfo()
    {
        await Task.Run(() =>
        {
            var os = Environment.OSVersion;
            var version = os.Version;
            switch (version.Major)
            {
                case 10 when version.Build >= 19041:
                    Log.Fatal("Windows Version: Windows 10 {OsVersion}", version.Build);
                    break;
                case 10 when version.Build >= 22000:
                    Log.Fatal("Windows Version: Windows 11 {OsVersion}", version.Build);
                    break;
                default:
                    Log.Fatal("Windows Version: {OsVersion}", Environment.OSVersion);
                    break;
            }

            Log.Fatal(".NET SDK Version: {Version}", Environment.Version);

            // Query CPU
            var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (var o in searcher.Get())
            {
                var share = (ManagementObject)o;
                Log.Fatal("CPU: {Unknown}", share["Name"]);
            }

            // Query Graphics Card
            searcher = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (var o in searcher.Get())
            {
                var share = (ManagementObject)o;
                Log.Fatal("Graphics Card: {Unknown}", share["Name"]);
            }

            // Query Memory
            searcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            foreach (var o in searcher.Get())
            {
                var share = (ManagementObject)o;
                var capacityBytes = (ulong)share["Capacity"];
                var mem = (double)capacityBytes / 1024 / 1024 / 1024;
                Log.Fatal("Memory: {Unknown} GB", mem);
            }
        });
    }

    #endregion
}