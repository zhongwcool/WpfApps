using System.Windows.Media;

namespace App18.Material.Models;

public class ColorPreset
{
    public Color Primary { get; set; } = Colors.Gold;
    public Color Secondary { get; set; } = Colors.LimeGreen;
    public string Name { get; set; } = string.Empty;
}