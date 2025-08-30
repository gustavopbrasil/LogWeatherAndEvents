using LogWeatherAndEvents.Model;

namespace LogWeatherAndEvents.Services;

public interface IWeatherService
{
    /// Fetches current weather for a given city/country and maps it into a DTO.
        Task<WeatherEntity> GetCurrentAsync(string city);
}
