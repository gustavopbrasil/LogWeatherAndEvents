// /Services/IEventPublisher.cs
using System.Threading;
using System.Threading.Tasks;
using LogWeatherAndEvents.Model;

public interface IEventPublisher
{
    Task PublishRainyDayAsync(WeatherEntity data);
}
