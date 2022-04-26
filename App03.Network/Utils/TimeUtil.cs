using System;

namespace App03.Network.Utils;

public static class TimeUtil
{
    private const string TimeStampFormat = "HH:mm:ss.fff";

    public static string GetTimeStamp()
    {
        return DateTime.Now.ToString(TimeStampFormat);
    }
}