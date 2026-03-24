using DataLayer;
using Microsoft.AspNetCore.Mvc;
using PowerTestTask.Server.Dto;

namespace PowerTestTask.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    [HttpGet("current")]
    public async Task<CurrentDto> Current([FromQuery] string city = "Moscow")
    {
        _logger.LogInformation("current");
        var current = await _weatherService.GetCurrent(city);        

        CurrentDto result = new()
        {
            TemperatureC = current.Current.Temperature
        };
        return result;
    }

    [HttpGet("forecast/hours")]
    public async Task<HourForecastDto> ForecastHours([FromQuery] string city = "Moscow")
    {
        _logger.LogInformation("forecast/hours");
        var forecast = await _weatherService.GetForecast(city);        
        var days = forecast.Forecast.ForecastDay.ToArray();
        var todayHours = days[0].Hours.ToList();

        var currentHour = DateTime.Now.Hour;
        var index = todayHours.FindIndex(hour => hour.Hour.EndsWith($"{currentHour}:00"));

        var tomorrowHours = days[1].Hours.ToList();

        HourForecastDto result = new()
        {
            Today = [.. todayHours.Skip(index).Select(h => new HourDto
            {
                Hour = h.Hour,
                Temperature = h.TemperatureC,
            })],
            Tomorrow = [.. tomorrowHours.Select(h => new HourDto
            {
                Hour = h.Hour,
                Temperature = h.TemperatureC,
            })]
        };
        return result;
    }

    [HttpGet("forecast/days")]
    public async Task<DaysForecastsDto> ForecastDays([FromQuery] string city = "Moscow")
    {
        _logger.LogInformation("forecast/days");
        var forecast = await _weatherService.GetForecast(city);        
        var days = forecast.Forecast.ForecastDay.ToArray();

        DaysForecastsDto result = new()
        {
            TomorrowTemperature = days.Length >= 2 ? days[1].Day.AvgTemperature : null,
            DayAfterTomorrowTemperature = days.Length >= 3 ? days[2].Day.AvgTemperature : null,
            TwoDaysAfterTomorrowTemperature = days.Length >= 4 ? days[3].Day.AvgTemperature : null,
        };
        return result;
    }
}

