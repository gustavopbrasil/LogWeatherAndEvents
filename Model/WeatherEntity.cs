using Azure;
using Azure.Data.Tables;

public class WeatherEntity : ITableEntity
{
    // Required by ITableEntity
    public string PartitionKey { get; set; } = default!;
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    // Your data
    public double Temperature { get; set; }
    public string? Condition { get; set; }
    public bool IsRainyDay { get; set; }
    public DateTime LoggedAt { get; set; }

    public WeatherEntity() { }

    public WeatherEntity(string city, string country)
    {
        PartitionKey = country;                 // batch-friendly key
        RowKey = $"{city}_{Guid.NewGuid()}";    // unique row id
        LoggedAt = DateTime.UtcNow;
    }
}
