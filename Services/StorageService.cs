using Azure;
using Azure.Data.Tables;


public class StorageService : IStorageService
{
    private readonly TableServiceClient _serviceClient;

    public StorageService(string? connectionString = null)
    {
        connectionString ??= Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        _serviceClient = new TableServiceClient(connectionString);
    }

    public async Task SaveWeatherAsync(IEnumerable<WeatherEntity> items, string tableName = "Weather")
    { 
        var tableClient = _serviceClient.GetTableClient(tableName);
        await tableClient.CreateIfNotExistsAsync();

        // Group by PartitionKey so we can use transactional batches
        foreach (var group in items.GroupBy(e => e.PartitionKey))
        {
            var batch = new List<TableTransactionAction>(100);

            foreach (var entity in group)
            {
                batch.Add(new TableTransactionAction(TableTransactionActionType.Add, entity));

                // Submit every 100 (service limit per batch)
                if (batch.Count == 100)
                {
                    await tableClient.SubmitTransactionAsync(batch);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
                await tableClient.SubmitTransactionAsync(batch);
        }
    }
}
