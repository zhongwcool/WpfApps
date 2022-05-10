namespace App10.Weather.Models;

/// <summary>
/// 24小时逐小时天气预报
/// <remarks>doc：https://seniverse.yuque.com/docs/share/2fe8443e-9b8d-4906-92e8-04915b04bde9</remarks>
/// </summary>
public class HourlyModel
{
    public LocationInfo LocationInfo { set; get; }
    public HourlyInfo Hourly { set; get; }
    public string Last_Update { set; get; }
}