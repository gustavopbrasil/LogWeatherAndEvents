
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using LogWeatherAndEvents.Services;

public class EventGridPublisher : IEventPublisher
{
    private readonly EventGridPublisherClient _client;

    public EventGridPublisher()
    {
        var endpoint = new Uri(Environment.GetEnvironmentVariable("EventGrid:TopicEndpoint"));

        var key = Environment.GetEnvironmentVariable("EventGrid:TopicKey");


        _client = new EventGridPublisherClient(endpoint, new AzureKeyCredential(key),
            new EventGridPublisherClientOptions
            {
                Retry =
                {
                    Mode = Azure.Core.RetryMode.Exponential,
                    MaxRetries = 5
                }
            });
    }

    public async Task PublishRainyDayAsync(WeatherEntity data)
    {
        var ev = new EventGridEvent(
            subject: $"/weather/{data.Country}/{data.City}",
            eventType: "weather.rainyday.detected",
            dataVersion: "1.0",
            data: data);

        await _client.SendEventAsync(ev);
    }
}
