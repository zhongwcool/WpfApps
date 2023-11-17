using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using App12.SQLite.HotelDbContext;
using Mar.Cheese;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace App12.SQLite;

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
        SystemUtil.PrintSystemInfo(OutOptions.UseSerilog);
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
}