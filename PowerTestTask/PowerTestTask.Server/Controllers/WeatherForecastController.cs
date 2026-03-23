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

        var response = await client.GetAsync($"?key={_forecastSettings.Key}&q={coordinates.Latitude},{coordinates.Longitude}&days=3").ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var current = JsonSerializer.Deserialize<WeatherApiDto>(content);

        HourForecastDto result = new()
        {
            Today = current.Forecast.ForecastDay.First().Hours.Select(h => new HourDto
            {
                Hour = h.Hour,
                Temperature = h.TemperatureC,
            }).ToArray(),
            Tomorrow = current.Forecast.ForecastDay.First().Hours.Select(h => new HourDto
            {
                Hour = h.Hour,
                Temperature = h.TemperatureC,
            }).ToArray()
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
            TomorrowTemperature = days[1].Day.AvgTemperature,
            DayAfterTomorrowTemperature = days[2].Day.AvgTemperature,
            TwoDaysAfterTomorrowTemperature = days[3].Day.AvgTemperature,
        };
        return result;
    }
}

