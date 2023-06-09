using System.IO;
using Newtonsoft.Json;

namespace App16.Python.Utils;

public static class JsonUtil
{
    /// <summary>
    /// save data to json file
    /// </summary>
    /// <param name="filename">target file</param>
    /// <param name="json">json data</param>
    public static void Save(string filename, string json)
    {
        using var fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var sw = new StreamWriter(fs);
        sw.WriteLine(json);
        sw.Flush();
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// Load data from json
    /// </summary>
    public static T? Load<T>(string filename)
    {
        using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs);
        var json = sr.ReadToEnd();
        sr.Close();
        fs.Close();

        if (string.IsNullOrEmpty(json)) return default;
        var model = JsonConvert.DeserializeObject<T>(json);
        return model;
    }
}