using System;

namespace App18.Material.Models;

public class MyApp
{
    public ColorPreset Preset { get; set; } = new();
    public DateTime LastModified { get; set; } = DateTime.Now;
}