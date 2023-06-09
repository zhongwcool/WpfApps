using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using App17.Login.Models;
using App17.Login.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace App17.Login.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Prepare();
    }

    public async void RequestWater()
    {
        // 创建新的 HttpClient 对象，并设置 Cookie
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Cookie", new[] { _session.Cookie });

        // 发送请求并解析响应
        // 构造登录请求参数
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("deviceId", _session.DeviceId)
        });

        var response = await client.PostAsync(_session.WaterApi, content);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Water = JsonConvert.DeserializeObject<ZyWater>(result) ?? new ZyWater();

        TxtRepo = result; // 处理数据
    }

    private ZyWater _water;

    public ZyWater Water
    {
        get => _water;
        set => SetProperty(ref _water, value);
    }

    public async void Login2()
    {
        var handler = new HttpClientHandler
            { UseCookies = true, CookieContainer = new CookieContainer(), AllowAutoRedirect = true };
        using var client = new HttpClient(handler);

        // 发送登录请求
        // 构造登录请求参数
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("userPhone", _session.UserPhone),
            new KeyValuePair<string, string>("userPwd", _session.UserPwd)
        });
        var response = await client.PostAsync(_session.LoginApi, content);
        // 保存 Cookie，以便后续请求使用
        var cookies = response.Headers.GetValues("Set-Cookie").ToArray();
        _session.Cookie = cookies[0].Split(';')[0];
        var model = new ZySessionModel
        {
            Sessions = new List<ZySession> { _session },
            Last_Update = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        var json = JsonConvert.SerializeObject(model);
        JsonUtil.Save(JSON_FILE, json);

        if (response.StatusCode == HttpStatusCode.Found)
        {
            Console.WriteLine(response.Headers.Location);
            // 重定向到登录页面
            var response2 = await client.PostAsync(_session.RedirApi, content);
            response2.EnsureSuccessStatusCode();
            var result2 = await response2.Content.ReadAsStringAsync();
            TxtRepo = result2; // 处理数据
            return;
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        TxtRepo = result; // 处理数据
    }

    private string _txtRepo = "Hello World";

    public string TxtRepo
    {
        get => _txtRepo;
        set => SetProperty(ref _txtRepo, value);
    }

    #region ZySession

    private ZySession _session = new();

    private void Prepare()
    {
        var model = JsonUtil.Load<ZySessionModel>(JSON_FILE);
        if (model == null) return;
        var list = model.Sessions;
        _session = list[0];
        if (string.IsNullOrEmpty(_session.Cookie)) Login2();
    }

    private const string JSON_FILE = "sessions.json";

    #endregion
}