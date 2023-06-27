using System.Collections.Generic;

namespace App02.TTS.Models;

public class Role
{
    public string Name { get; set; } = string.Empty;
    public string VoiceKey { get; set; } = string.Empty;
    public string VoiceName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class RoleModel
{
    public List<Role> Roles { set; get; }
    public string Last_Update { set; get; }
}