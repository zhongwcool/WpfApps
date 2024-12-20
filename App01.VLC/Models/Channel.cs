﻿using System.ComponentModel.DataAnnotations.Schema;

namespace App01.VLC.Models;

public class Channel
{
    public string GroupTitle { get; set; }
    public string TvgId { get; set; }
    public string TvgLogo { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    [NotMapped] public string Delay { get; set; }
    public bool HasStar { get; set; }
}