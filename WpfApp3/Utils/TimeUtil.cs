using System;

namespace WpfApp3.Utils;

public static class TimeUtil
{
    private const string TimeStampFormat = "HH:mm:ss.fff";

    public static string GetTimeStamp()
    {
        return DateTime.Now.ToString(TimeStampFormat);
    }
}