using System;
using System.IO;

namespace App11.HIK.Utils;

public static class FileUtil
{
    private const string FormatFileRecord = "MMdd-HHmm-ss";
    private const string FormatFileShot = "MMdd-HHmmss-fff";
    private static string _basePath = Path.Combine(Environment.CurrentDirectory, "01-Shot");

    public static string GetRecordFilename()
    {
        Directory.CreateDirectory(_basePath); //如果文件夹不存在，则创建一个新的
        return Path.Combine(_basePath, DateTime.Now.ToString(FormatFileRecord) + ".mp4");
    }

    public static string GetShotFilename()
    {
        Directory.CreateDirectory(_basePath); //如果文件夹不存在，则创建一个新的
        return Path.Combine(_basePath, DateTime.Now.ToString(FormatFileShot) + ".jpg");
    }

    public static bool SetStorePath(string storePath)
    {
        try
        {
            Directory.CreateDirectory(storePath); //如果文件夹不存在，则创建一个新的
            _basePath = storePath;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}