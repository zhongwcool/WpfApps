using System.Collections.Generic;

namespace App11.HIK.Models;

public class Device
{
    public string MN { set; get; }
    public string SN { set; get; }
    public string IP { set; get; }
    public string NN { set; get; }
    public string MC { set; get; }
    public int TP { set; get; }
    public string NT { set; get; }
    public string FV { set; get; }
    public string PV { set; get; }
}

public class DeviceModel
{
    public List<Device> Devices { set; get; }
    public string Last_Update { set; get; }
}