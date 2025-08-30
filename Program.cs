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
        var config = context.Configuration;


        services.AddOptions<WeatherOptions>().Configure(opts =>
        {
            opts.ApiKey  = config["Weather:ApiKey"]        ?? config["Values:Weather__ApiKey"]  ?? "";
            opts.BaseUrl = config["Weather:BaseUrl"]       ?? config["Values:Weather__BaseUrl"] ?? "https://api.weatherapi.com/v1/";
        });

        services.AddHttpClient<IWeatherService, WeatherService>((sp, client) =>
        {
            var o = sp.GetRequiredService<IOptions<WeatherOptions>>().Value;
            client.BaseAddress = new Uri(o.BaseUrl);
            client.Timeout     = TimeSpan.FromSeconds(10);
        });

        var storageConn =
            config["AzureWebJobsStorage"] ??
            config["Values:AzureWebJobsStorage"]; // when running via local.settings.json

        // services.AddSingleton(new BlobServiceClient(storageConn));
        // services.AddSingleton(new TableServiceClient(storageConn)); // keep if you use Tables

        // // 4) Your app services
        // services.AddSingleton<IStorageService, StorageService>();
        // If you also created a table service, register it too:
        // services.AddSingleton<ITableStorageService, TableStorageService>();
    })
    .Build();

host.Run();
