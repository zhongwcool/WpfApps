namespace App10.Weather.Models;

public class WeatherModel
{
    public Location location { set; get; }
    public Weather now { set; get; }
    public string last_update { set; get; }
}

public class Location
{
    public string id { set; get; }

    public string name { set; get; }

    public string country { set; get; }

    public string path { set; get; }

    public string timezone { set; get; }

    public string timezone_offset { set; get; }
}

public class Weather
{
    public string text { set; get; }

    public string code { set; get; }

    public string temperature { set; get; }

    public string humidity { set; get; }

    public string wind_direction { set; get; }

    public string wind_direction_degree { set; get; }

    public string wind_speed { set; get; }

    public string wind_scale { set; get; }
}