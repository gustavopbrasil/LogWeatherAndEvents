namespace LogWeatherAndEvents.Model.API;

public class WeatherOptions
{
    public string ApiKey { get; set; } = "";
    public string BaseUrl { get; set; } = "https://api.weatherapi.com/v1/";
}
