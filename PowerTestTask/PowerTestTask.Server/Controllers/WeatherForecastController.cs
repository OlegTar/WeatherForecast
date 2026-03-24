using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerTestTask.Server.Configuration;
using PowerTestTask.Server.Dto;
using System.Text.Json;

namespace PowerTestTask.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly CityCoordinates _configuration;
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ForecastSettings _forecastSettings;

    public WeatherForecastController(IOptions<CityCoordinates> configuration,
        IOptions<ForecastSettings> forecastSettings,
        ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration.Value;
        _httpClientFactory = httpClientFactory;
        _forecastSettings = forecastSettings.Value;
    }

    [HttpGet("current")]
    public async Task<CurrentDto> Current([FromQuery] string city = "Moscow")
    {
        var coordinates = _configuration.Cities[city];

        using var client = _httpClientFactory.CreateClient("current");
        var response = await client.GetAsync($"?key={_forecastSettings.Key}&q={coordinates.Latitude},{coordinates.Longitude}");
        var content = await response.Content.ReadAsStringAsync();
        var current = JsonSerializer.Deserialize<WeatherApiDto>(content);

        CurrentDto result = new()
        {
            TemperatureC = current.Current.Temperature
        };
        return result;
    }

    [HttpGet("forecast/hours")]
    public async Task<HourForecastDto> ForecastHours([FromQuery] string city = "Moscow")
    {
        var coordinates = _configuration.Cities[city];

        using var client = _httpClientFactory.CreateClient("forecast");

        var response = await client.GetAsync($"?key={_forecastSettings.Key}&q={coordinates.Latitude},{coordinates.Longitude}&days=2").ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var current = JsonSerializer.Deserialize<WeatherApiDto>(content);
        var days = current.Forecast.ForecastDay.ToArray();
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
        var coordinates = _configuration.Cities[city];

        using var client = _httpClientFactory.CreateClient("forecast");
        var response = await client.GetAsync($"?key={_forecastSettings.Key}&q={coordinates.Latitude},{coordinates.Longitude}&days=4");
        var content = await response.Content.ReadAsStringAsync();
        var current = JsonSerializer.Deserialize<WeatherApiDto>(content);
        var days = current.Forecast.ForecastDay.ToArray();

        DaysForecastsDto result = new()
        {
            TomorrowTemperature = days.Length >= 2 ? days[1].Day.AvgTemperature : null,
            DayAfterTomorrowTemperature = days.Length >= 3 ? days[2].Day.AvgTemperature : null,
            TwoDaysAfterTomorrowTemperature = days.Length >= 4 ? days[3].Day.AvgTemperature : null,
        };
        return result;
    }
}

