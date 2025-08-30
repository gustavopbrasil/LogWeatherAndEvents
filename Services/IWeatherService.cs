using LogWeatherAndEvents.Model;

namespace LogWeatherAndEvents.Services;

public interface IWeatherService
{
    Task<WeatherEntity> GetCurrentAsync(string city);
}
