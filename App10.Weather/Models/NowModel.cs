namespace App10.Weather.Models;

/// <summary>
/// 天气实况
/// <remarks>doc：https://seniverse.yuque.com/books/share/ded1e167-e35c-4669-8306-cf65c6e01dc0/wvfkgq</remarks>
/// </summary>
public class NowModel
{
    public LocationInfo Location { set; get; }
    public NowInfo Now { set; get; }
    public string Last_Update { set; get; }
}