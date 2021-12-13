using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WpfApp3.Utils;

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
}