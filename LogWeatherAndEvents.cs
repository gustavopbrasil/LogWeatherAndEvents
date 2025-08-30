using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using LogWeatherAndEvents.Services;
using Newtonsoft.Json;
using Microsoft.Identity.Client.Extensions.Msal;

namespace WeatherMicroService;

public class LogWeatherAndEvents
{
    private readonly ILogger<LogWeatherAndEvents> _logger;
    private readonly IWeatherService _weather;
    
    private readonly CloudTable _table;
    public LogWeatherAndEvents(ILogger<LogWeatherAndEvents> logger, IWeatherService weather)
    {
        _logger = logger;
        _weather = weather;

    }

    

    [Function("LogWeatherAndEvents")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, ILogger log)
    {
        _logger.LogInformation("Log Weather Service Has Begun.");

        var locations = new[]
        {
            new { City = "Medellin", Country = "Colombia" },
            new { City = "Charleston", Country = "US" },
            new { City = "London", Country = "UK" },
            new { City = "Lisbon", Country = "Portugal" },
            new { City = "Campinas", Country = "Brazil" }
        };
        // MIGRATE THIS TO A DYNAMIC TABLE/INPUT 

        List<WeatherEntity> weatherResults = new List<WeatherEntity>();
        foreach (var loc in locations)
        {

            var result = await _weather.GetCurrentAsync(loc.City);
            weatherResults.Add(result);
        }

        var storage = new StorageService();
        await storage.SaveWeatherAsync(weatherResults);
        

// //IMPLEMENT EXCEPTION HANDLING
        // TRY CATCH & RETRY FOR EXTERNAL API 

        return new OkObjectResult("Weather logged successfully.");
    }
}


   


