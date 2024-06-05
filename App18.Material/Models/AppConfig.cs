using System;
using System.IO;
using System.Reflection;
using Mar.Cheese;

namespace App18.Material.Models;

public class AppConfig : MyApp
{
    private AppConfig()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var assembly = Assembly.GetEntryAssembly()?.GetName();
        var target = null == assembly || string.IsNullOrEmpty(assembly.Name) ? "md3" : assembly.Name;
        var myAppFolder = Path.Combine(appDataPath, target);
        var jsonFile = Path.Combine(myAppFolder, "app.json");

        try
        {
            var model = JsonUtil.Load<MyApp>(jsonFile);
            if (model != null) CopyProperties(model, this);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static AppConfig _instance;

    public static AppConfig CreateInstance()
    {
        _instance ??= new AppConfig();
        return _instance;
    }

    private static void CopyProperties(MyApp source, MyApp destination)
    {
        var properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.CanRead || !property.CanWrite) continue;
            var value = property.GetValue(source);
            property.SetValue(destination, value);
        }
    }

    // 保存用户选择
    public void Save()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var assembly = Assembly.GetEntryAssembly()?.GetName();
        var target = null == assembly || string.IsNullOrEmpty(assembly.Name) ? "md3" : assembly.Name;
        var myAppFolder = Path.Combine(appDataPath, target);
        Directory.CreateDirectory(myAppFolder);
        var jsonFile = Path.Combine(myAppFolder, "app.json");
        _instance.LastModified = DateTime.Now;
        JsonUtil.Save<MyApp>(jsonFile, this);
    }
}