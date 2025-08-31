public interface IStorageService
{
  /// Saves the given weather reading by mapping it to a "WeatherEntity"/>.
  Task SaveWeatherAsync(WeatherEntity entity, string tableName = "Weather");
  

}
