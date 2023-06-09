using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using App17.Login.Models;
using App17.Login.Utils;
using Newtonsoft.Json;

namespace App17.Login.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Prepare();
    }

    private async void RequestDataPost(ZySession session)
    {
        // 创建新的 HttpClient 对象，并设置 Cookie
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Cookie", new[] { _session.Cookie });

        // 发送请求并解析响应
        // 构造登录请求参数
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("deviceId", session.DeviceId)
        });

        var response = await client.PostAsync(session.WaterApi, content);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        TxtRepo.Text = result; // 处理数据
    }

    private async void Login2(ZySession session)
    {
        var handler = new HttpClientHandler
            { UseCookies = true, CookieContainer = new CookieContainer(), AllowAutoRedirect = true };
        using var client = new HttpClient(handler);

        // 发送登录请求
        // 构造登录请求参数
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("userPhone", session.UserPhone),
            new KeyValuePair<string, string>("userPwd", session.UserPwd)
        });
        var response = await client.PostAsync(session.LoginApi, content);
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
            var response2 = await client.PostAsync(session.RedirApi, content);
            response2.EnsureSuccessStatusCode();
            var result2 = await response2.Content.ReadAsStringAsync();
            TxtRepo.Text = result2; // 处理数据
            return;
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        TxtRepo.Text = result; // 处理数据
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Login2(_session);
    }

    private void ButtonData_OnClick(object sender, RoutedEventArgs e)
    {
        RequestDataPost(_session);
    }

    #region ZySession

    private ZySession _session;

    private void Prepare()
    {
        var list = ReadData();
        _session = list[0];
        if (string.IsNullOrEmpty(_session.Cookie)) Login2(_session);
    }

    private static List<ZySession> ReadData()
    {
        var model = JsonUtil.Load<ZySessionModel>(JSON_FILE);
        return model?.Sessions;
    }

    private const string JSON_FILE = "sessions.json";

    #endregion
}