public interface IStorageService
{
      /// Saves the given weather reading by mapping it to a "WeatherEntity"/>.
    Task SaveWeatherAsync(IEnumerable<WeatherEntity> items, string tableName = "Weather");
} 
