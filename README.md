# LogWeatherAndEvents

Azure Functions (.NET 6) that fetch weather data and log it as events into Azure Table Storage.

## ✨ What it does
- Exposes an HTTP endpoint to retrieve current weather for a city/country.
- Persists a normalized `WeatherEntity` into Azure Table Storage for auditing/analytics.

## 🧱 Architecture (high level)
- **Function**: `LogWeatherAndEvents` (HTTP trigger, thin)
- **Services**:
  - `IWeatherService` / `WeatherService`: calls external Weather API and returns a DTO.
  - `IStorageService` / `StorageService`: maps to `WeatherEntity` and writes to Table Storage.
- **Model**:
  - `WeatherEntity`: Azure Tables row, partitioned by country, row key: `"{city}_{Guid}"`.
  - Weather API DTOs (nullable-friendly).

## 📁 Project structure
/Model
WeatherEntity.cs
API/WeatherResponse.cs (nullable props)
WeatherResult.cs (DTO returned to clients)
...
/Services
IWeatherService.cs
WeatherService.cs
IStorageService.cs
StorageService.cs
LogWeatherAndEvents.cs <- HTTP function (entry point)
Program.cs <- Host/DI configuration
host.json
local.settings.json <- (not committed) local config
