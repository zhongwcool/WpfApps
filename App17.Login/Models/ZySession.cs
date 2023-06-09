using System.Collections.Generic;

namespace App17.Login.Models;

public class ZySession
{
    public string Cookie { set; get; }
    public string UserPhone { set; get; }
    public string UserPwd { set; get; }
    public string DeviceId { set; get; }
    public string LoginApi { set; get; }
    public string RedirApi { set; get; }
    public string WaterApi { set; get; }
}

public class ZySessionModel
{
    public List<ZySession> Sessions { set; get; }
    public string Last_Update { set; get; }
}