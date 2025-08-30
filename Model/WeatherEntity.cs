using Azure;
using Azure.Data.Tables;

    /// Row stored in Azure Table Storage representing a single weather reading.
    /// Partitioned by country to optimize queries per country.

public class WeatherEntity : ITableEntity
{

    // Required by ITableEntity
    public string PartitionKey { get; set; } = default!;
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
 
 
    public double Temperature { get; set; }
    public string? Condition { get; set; }
    public bool IsRainyDay { get; set; }
    public DateTime LoggedAt { get; set; }

    public WeatherEntity() { }


     /// Creates a new WeatherEntity with a country-based PartitionKey and a RowKey including the city and an auto-generated Guid.
     /// <param name="city">City name (used in RowKey prefix).</param>
    /// <param name="country">Country code/name (used as PartitionKey).</param>
    public WeatherEntity(string city, string country)
    {
        PartitionKey = country;
        RowKey = $"{city}_{Guid.NewGuid()}";
        LoggedAt = DateTime.UtcNow;
    }
}
