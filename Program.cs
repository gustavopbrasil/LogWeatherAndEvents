using System;
// using Azure.Data.Tables;
// using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using LogWeatherAndEvents.Model.API;
using LogWeatherAndEvents.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        cfg.AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddHttpClient<IWeatherService, WeatherService>();
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<IEventPublisher, EventGridPublisher>();


    })
    .Build();

host.Run();
