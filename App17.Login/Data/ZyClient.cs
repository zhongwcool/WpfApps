using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Timers;
using App17.Login.Models;
using Mar.Cheese;
using Newtonsoft.Json;

namespace App17.Login.Data;

public class ZyClient
{
    private readonly Timer _timer = new();

    public ZyClient()
    {
        Prepare();
    }

    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        // 执行需要定时执行的任务
        RequestWater();
    }

    private async void RequestWater()
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

        //返回302代表session失效
        if (response.StatusCode == HttpStatusCode.Found)
        {
            LoginCompleted = RequestWater;
            Login2();
            return;
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        var water = JsonConvert.DeserializeObject<ZyWater>(result) ?? new ZyWater();
        water.Html = result; // 处理数据
        DataReceived.Invoke(water);
    }

    private async void Login2()
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
        JsonUtil.Save(JSON_FILE, model);

        if (response.StatusCode == HttpStatusCode.Found)
        {
            Console.WriteLine(response.Headers.Location);
            // 重定向到登录页面
            var response2 = await client.PostAsync(_session.RedirApi, content);
            response2.EnsureSuccessStatusCode();
            var result2 = await response2.Content.ReadAsStringAsync();
            // TxtRepo = result2; // 处理数据
            LoginCompleted.Invoke();
            return;
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        // TxtRepo = result; // 处理数据
        LoginCompleted.Invoke();
    }

    public delegate void OnDataReceived(ZyWater water);

    public event OnDataReceived DataReceived;

    private delegate void OnLoginCompleted();

    private event OnLoginCompleted LoginCompleted;

    #region ZySession

    private ZySession _session = new();

    private void Prepare()
    {
        var model = JsonUtil.Load<ZySessionModel>(JSON_FILE);
        if (model == null) return;
        var list = model.Sessions;
        _session = list[0];

        // 设置时间间隔为 1 秒（1000 毫秒）
        _timer.Interval = 60000;
        // 绑定事件处理程序
        _timer.Elapsed += OnTimedEvent;
        _timer.Start();
    }

    private const string JSON_FILE = "sessions.json";

    #endregion
}