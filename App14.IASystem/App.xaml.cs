using System;
using System.IO;
using System.Management;
using System.Windows;
using Serilog;

namespace App14.IASystem;

public partial class App : Application
{
    private readonly string _file = Path.Combine("03-ToolLog", "log.txt");

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

    private static void PrintSystemInfo()
    {
        Log.Fatal("Windows Version: {OsVersion}", Environment.OSVersion);
        Log.Fatal(".NET SDK Version: {Version}", Environment.Version);

        ManagementObjectSearcher searcher;

        // Query CPU
        searcher = new ManagementObjectSearcher("select * from Win32_Processor");
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
            Log.Fatal("Graphics Card: " + share["Name"]);
        }

        // Query Memory
        searcher = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
        foreach (var o in searcher.Get())
        {
            var share = (ManagementObject)o;
            var capacityBytes = (ulong)share["Capacity"];
            var mem = (double)capacityBytes / 1024 / 1024 / 1024;
            Log.Fatal("Memory: " + mem + "GB");
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        Log.CloseAndFlush();
    }
}