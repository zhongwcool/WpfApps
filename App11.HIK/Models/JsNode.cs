using System.Collections.Generic;
using System.Threading.Tasks;
using App11.HIK.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace App11.HIK.Models;

public class JsNode : ObservableObject
{
    private string _modelType; // 设备类型

    public string ModelType
    {
        get => _modelType;
        set => SetProperty(ref _modelType, value);
    }

    private string _modelNum;

    public string ModelNum
    {
        get => _modelNum;
        set => SetProperty(ref _modelNum, value);
    }

    private string _serialNum;

    public string SerialNum
    {
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

    private string _mac; // mac地址

    public string Mac
    {
        get => _mac;
        set => SetProperty(ref _mac, value);
    }

    private int _tcpPort; // 设备类型

    public int TcpPort
    {
        get => _tcpPort;
        set => SetProperty(ref _tcpPort, value);
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

    public static async Task<List<JsNode>> CreateDummy()
    {
        var robots = new List<JsNode>();
        await Task.Run(() =>
        {
            var data = JsonUtil.LoadDataFromJson(JsonFile);
            if (string.IsNullOrEmpty(data)) return;
            var model = JsonConvert.DeserializeObject<DeviceModel>(data);
            if (null == model) return;

            foreach (var soul in model.Devices)
            {
                robots.Add(new JsNode
                {
                    ModelType = soul.NT,
                    ModelNum = soul.MN,
                    SerialNum = soul.SN,
                    NodeName = soul.NN,
                    Mac = soul.MC,
                    TcpPort = soul.TP,
                    DevIp = soul.IP,
                    Firmware = soul.FV,
                    Protocol = soul.PV
                });
            }
        });

        return robots;
    }

    private const string JsonFile = "data_devices.json";

    #endregion
}