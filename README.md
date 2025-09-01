# LogWeatherAndEvents

Azure Functions (.NET 9) that fetch weather data and log it into Azure Storage.

##  What it does
- Exposes an HTTP endpoint to retrieve current weather for a city.
- Persists a normalized `WeatherEntity` into Azure Table Storage for.
  *For "Rainy" weather publishes a Event into Azure Event Grid.

## Architecture (high level)
- **Function**: `LogWeatherAndEvents` (HTTP trigger, thin)
- **Services**:
  - `IWeatherService` / `WeatherService`: calls external Weather API and returns a DTO.
  - `IStorageService` / `StorageService`: maps to `WeatherEntity` and writes to Table Storage.
  - `IEventPublisher` /  `EventGridPublisher`: publishes "rainy" event into Azure Grid.
- **Model**:
  - `WeatherEntity`: Azure Tables row, partitioned by country, row key: `"{city}_{Guid}"`.
  - Weather API DTOs (nullable-friendly).

## Project structure
/Model
WeatherEntity.cs
API/
WeatherResponse.cs (nullable props)
...
/Services
IEventPublisher.cs
EventGridPublisher.cs
IWeatherService.cs
WeatherService.cs
IStorageService.cs
StorageService.cs
...
LogWeatherAndEvents.cs <- HTTP function (entry point)
Program.cs <- Host/DI configuration
host.json
local.settings.json <- (not committed) local config


## Requirements
- .NET 9 SDK
- Azure Functions Core Tools v4
- Azurite (recommended for local Table/Blob/Queue)
- An API key/URL for your weather provider

## Configuration (`local.settings.json`)
Create a `local.settings.json` (do **not** commit it):

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "Weather__BaseUrl": "https://api.weatherapi.com/v1",
    "Weather__ApiKey": "<your_api_key>",
  }
}

