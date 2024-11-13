using App22.Selected.Enums;

namespace App22.Selected.Models;

public class StatusItem
{
    public LockStatus Status { get; set; } = LockStatus.Unknown;
    public string IconKey { get; set; }
}