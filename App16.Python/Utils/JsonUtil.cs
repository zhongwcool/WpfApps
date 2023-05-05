using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace App16.Python.Utils;

public static class JsonUtil
{
    /// <summary>
    /// save data to json file
    /// </summary>
    /// <param name="filename">target file</param>
    /// <param name="json">json data</param>
    public static void SaveDataToJson(string filename, string json)
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
    public static string LoadDataFromJson(string filename)
    {
        using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs);
        var json = sr.ReadToEnd();
        sr.Close();
        fs.Close();

        var jObject = JObject.Parse(json);
        var result0 = jObject["results"];
        var result1 = result0?.ToList().FirstOrDefault();
        return result1?.ToString();
    }
}