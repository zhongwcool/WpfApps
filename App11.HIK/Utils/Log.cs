using System;
using System.Diagnostics;
using System.Text;

namespace App11.HIK.Utils;

public static class Log
{
    public static void D(string content)
    {
        var frames = new StackTrace().GetFrames();
        var caller = frames?.Length < 2 ? "Null" : frames?[1].GetMethod()?.Name;
        Serilog.Log.Debug("{@Caller}\t{@Content}", caller, content);
    }

    public static void I(Exception exception)
    {
        var content = GetExceptionInfo(exception);
        var frames = new StackTrace().GetFrames();
        var caller = frames?.Length < 2 ? "Null" : frames?[1].GetMethod()?.Name;
        Serilog.Log.Information("{@Caller}\t{@Content}", caller, content);
    }

    public static void I(string content)
    {
        var frames = new StackTrace().GetFrames();
        var caller = frames?.Length < 2 ? "Null" : frames?[1].GetMethod()?.Name;
        Serilog.Log.Information("{@Caller}\t{@Content}", caller, content);
    }

    public static void W(string content)
    {
        var frames = new StackTrace().GetFrames();
        var caller = frames?.Length < 2 ? "Null" : frames?[1].GetMethod()?.Name;
        Serilog.Log.Warning("{@Caller}\t{@Content}", caller, content);
    }

    public static void E(Exception exception)
    {
        var content = GetExceptionInfo(exception);
        var frames = new StackTrace().GetFrames();
        var caller = frames?.Length < 2 ? "Null" : frames?[1].GetMethod()?.Name;
        Serilog.Log.Error("{@Caller}\t{@Content}", caller, content);
    }

    public static void E(string content)
    {
        var frames = new StackTrace().GetFrames();
        var caller = frames?.Length < 2 ? "Null" : frames?[1].GetMethod()?.Name;
        Serilog.Log.Error("{@Caller}\t{@Content}", caller, content);
    }

    private static string GetExceptionInfo(Exception exception)
    {
        if (null == exception) return "exception is null";
        var builder = new StringBuilder();
        builder.AppendLine("异常信息：" + exception.Message);
        builder.AppendLine("   " + exception.GetType().FullName);
        builder.AppendLine("   " + exception.StackTrace.Trim());
        builder.AppendLine("   " + exception.TargetSite);

        return builder.ToString();
    }
}