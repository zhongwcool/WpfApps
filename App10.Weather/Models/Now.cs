﻿namespace App10.Weather.Models;

public class Now
{
    public string Text { set; get; } = "N/A";
    public string Code { set; get; } = "0";
    public string Temperature { set; get; } = "0";
    public string Feels_Like { set; get; }
    public string Pressure { set; get; }
    public string Humidity { set; get; }
    public string Visibility { set; get; }
    public string Wind_Direction { set; get; }
    public string Wind_Direction_Degree { set; get; }
    public string Wind_Speed { set; get; }
    public string Wind_Scale { set; get; }
    public string Clouds { set; get; }
    public string Dew_Point { set; get; }
}