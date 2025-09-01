using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using LogWeatherAndEvents.Model.API;

namespace LogWeatherAndEvents.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;
    }
        
    

    public async Task<WeatherEntity> GetCurrentAsync(string city)
    {
        
        try
        {
            // Weather API: map only what you need
            var url = $"{Environment.GetEnvironmentVariable("Weather__BaseUrl")}current.json?key={Environment.GetEnvironmentVariable("Weather__ApiKey")}&q={city}";
            var payload = await _http.GetFromJsonAsync<WeatherResponse>(url)
                          ?? throw new InvalidOperationException("Weather payload was null.");

            return new WeatherEntity(city, payload.Location.Country)
            {
                Country = payload.Location.Country ?? "",
                City = payload.Location.Name ?? "",
                Temperature = payload.Current.TempC ?? 0.0,
                Condition = payload.Current.Condition.Text ?? "",
                IsRainyDay = payload.Current.Condition.Text.ToLower().Contains("rain"),
                LoggedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString);
            throw ex;
        }
    }

    
}
