using System.ComponentModel;

namespace App14.IASystem.Enums;

public enum ModelType
{
    [Description("未知类型")] UNKNOWN = 0,
    [Description("WC设备")] WC,
    [Description("WQM设备")] WQM,
    [Description("AFE设备")] AFE,
    [Description("CCB设备")] CCB
}