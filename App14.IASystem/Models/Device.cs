using System;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App14.IASystem.Models;

public class Device : ObservableObject
{
    [Key] public Guid Id { get; set; }

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

    private DateTime? _addDate;

    public DateTime? AddDate
    {
        get => _addDate;
        set => SetProperty(ref _addDate, value);
    }

    private string _protocol; // 协议版本

    public string Protocol
    {
        get => _protocol;
        set => SetProperty(ref _protocol, value);
    }

    private Pool _pool;

    public virtual Pool Pool
    {
        get => _pool;
        set => SetProperty(ref _pool, value);
    }
}