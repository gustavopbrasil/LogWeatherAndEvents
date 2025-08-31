using Azure;
using Azure.Data.Tables;
using Azure.Messaging.EventGrid;


public class StorageService : IStorageService
{
    private readonly TableServiceClient _serviceClient;
    private readonly EventGridPublisherClient _eventClient;

    public StorageService(string? connectionString = null)
    {
        connectionString ??= Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        _serviceClient = new TableServiceClient(connectionString);
    }

    public async Task SaveWeatherAsync(WeatherEntity e, string tableName = "Weather")
    {
        var tableClient = _serviceClient.GetTableClient(tableName);
        await tableClient.CreateIfNotExistsAsync();
        await tableClient.UpsertEntityAsync(e);
    }


}
