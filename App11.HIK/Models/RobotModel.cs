using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace App11.HIK.Models;

public sealed class RobotModel : ObservableObject
{
    private int _type; // 设备类型

    public int Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private string _serialNum;

    public string SerialNum
    {
        // Serial number (hostname)
        get => _serialNum;
        set => SetProperty(ref _serialNum, value);
    }

    private string _devIp; // 设备IP

    public string DevIp
    {
        get => _devIp;
        set => SetProperty(ref _devIp, value);
    }

    private string _nodeName; // 用户标注

    public string NodeName
    {
        get => _nodeName;
        set => SetProperty(ref _nodeName, value);
    }

    private string _remoteMac; // mac地址

    public string RemoteMac
    {
        get => _remoteMac;
        set => SetProperty(ref _remoteMac, value);
    }

    private string _firmware; // 固件版本

    public string Firmware
    {
        get => _firmware;
        set => SetProperty(ref _firmware, value);
    }

    private string _protocol; // 协议版本

    public string Protocol
    {
        get => _protocol;
        set => SetProperty(ref _protocol, value);
    }

    #region Dummy

    public static RobotModel CreateDummy()
    {
        var rand = new Random();
        var index0 = rand.Next(0, 4);
        var index1 = rand.Next(0, 9);
        var soul = JsonConvert.DeserializeObject<Device>(Bar[index1]);
        if (null == soul) return null;
        return new RobotModel()
        {
            Type = index0,
            SerialNum = soul.SN,
            NodeName = soul.NM,
            RemoteMac = soul.MC,
            DevIp = soul.IP,
            Firmware = soul.FV,
            Protocol = soul.PV,
        };
    }

    private static readonly string[] Bar =
    {
        "{\"SN\":\"JSPRB00001\",\"NM\":\"哆啦A梦\",\"MC\":\"48:B0:2D:2F:8B:00\",\"IP\":\"192.168.7.196\",\"FV\":\"1.1\",\"PV\":\"0.1\"}",
        "{\"SN\":\"JSPRB00002\",\"NM\":\"哆啦B梦\",\"MC\":\"48:B0:2D:2F:8B:01\",\"IP\":\"192.168.7.196\",\"FV\":\"1.2\",\"PV\":\"0.1\"}",
        "{\"SN\":\"JSPRB00003\",\"NM\":\"哆啦C梦\",\"MC\":\"48:B0:2D:2F:8B:02\",\"IP\":\"192.168.7.196\",\"FV\":\"1.1\",\"PV\":\"0.2\"}",
        "{\"SN\":\"JSPRB00004\",\"NM\":\"哆啦D梦\",\"MC\":\"48:B0:2D:2F:8B:03\",\"IP\":\"192.168.7.196\",\"FV\":\"1.2\",\"PV\":\"0.2\"}",
        "{\"SN\":\"JSPRB00005\",\"NM\":\"哆啦E梦\",\"MC\":\"48:B0:2D:2F:8B:04\",\"IP\":\"192.168.7.196\",\"FV\":\"1.1\",\"PV\":\"0.3\"}",
        "{\"SN\":\"JSPRB00006\",\"NM\":\"哆啦F梦\",\"MC\":\"48:B0:2D:2F:8B:05\",\"IP\":\"192.168.7.196\",\"FV\":\"1.2\",\"PV\":\"0.5\"}",
        "{\"SN\":\"JSPRB00007\",\"NM\":\"漩涡鸣人\",\"MC\":\"48:B0:2D:2F:8B:06\",\"IP\":\"192.168.7.196\",\"FV\":\"1.1\",\"PV\":\"0.5\"}",
        "{\"SN\":\"JSPRB00008\",\"NM\":\"宇智波鼬\",\"MC\":\"48:B0:2D:2F:8B:07\",\"IP\":\"192.168.7.196\",\"FV\":\"1.3\",\"PV\":\"0.5\"}",
        "{\"SN\":\"JSPRB00009\",\"NM\":\"奇拉比\",\"MC\":\"48:B0:2D:2F:8B:08\",\"IP\":\"192.168.7.196\",\"FV\":\"1.4\",\"PV\":\"0.4\"}",
        "{\"SN\":\"JSPRB00010\",\"NM\":\"白\",\"MC\":\"48:B0:2D:2F:8B:10\",\"IP\":\"192.168.7.196\",\"FV\":\"1.1\",\"PV\":\"0.6\"}",
    };

    #endregion
}