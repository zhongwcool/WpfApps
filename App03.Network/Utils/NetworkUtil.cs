using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using App03.Network.Models;

namespace App03.Network.Utils;

public static class NetworkUtil
{
    public static NetworkInfo[] GetNetworkInfo()
    {
        var networkList = new List<NetworkInfo>();
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces(); //获取所有的网络接口
        foreach (var netInterface in networkInterfaces) //针对每张网卡
        {
            var ipInterfaceProperties = netInterface.GetIPProperties(); //获取描述此网络接口的配置的对象
            var uiaiCollection = ipInterfaceProperties.UnicastAddresses; //获取分配给此接口的单播地址
            foreach (var uiai in uiaiCollection) //针对每个IP
            {
                if (uiai.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                var ip = uiai.Address.ToString();
                if (ip == "127.0.0.1") continue;
                if (netInterface.OperationalStatus != OperationalStatus.Up) continue;
                //网卡已连接
                var info = new NetworkInfo { Ip = ip, Description = ip + "-" + netInterface.Description };
                networkList.Add(info);
            }
        }

        return networkList.ToArray();
    }

    public static string GetLocalIp()
    {
        Thread.Sleep(1000);
        var result = RunApp("route", "print");
        var match = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
        if (match.Success)
            return match.Groups[2].Value;
        try
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect("www.baidu.com", 80);
            var ip = ((IPEndPoint)tcpClient.Client.LocalEndPoint).Address.ToString();
            tcpClient.Close();
            return ip;
        }
        catch (Exception)
        {
            return "127.0.0.1";
        }
    }

    private static string RunApp(string filename, string arguments)
    {
        try
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = filename,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();

            using var streamReader = new StreamReader(process.StandardOutput.BaseStream, Encoding.Default);
            var result = streamReader.ReadToEnd();
            process.WaitForExit();
            streamReader.Close();
            return result;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}