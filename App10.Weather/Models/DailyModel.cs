using System.Collections.Generic;

namespace App10.Weather.Models;

/// <summary>
/// 未来15天逐日天气预报和昨日天气
/// <remarks>doc：https://seniverse.yuque.com/books/share/e52aa43f-8fe9-4ffa-860d-96c0f3cf1c49/sl6gvt</remarks>
/// </summary>
public class DailyModel
{
    public Location Location { set; get; }
    public List<Daily> Daily { set; get; }
    public string Last_Update { set; get; }
}