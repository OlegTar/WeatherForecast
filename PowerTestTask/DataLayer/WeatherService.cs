using DataLayer.Configuration;
using Microsoft.Extensions.Options;
using PowerTestTask.Server.Configuration;
using PowerTestTask.Server.Dto;
using System.Text.Json;

namespace DataLayer;

public class WeatherService : IWeatherService
{
    private readonly CityCoordinates _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ForecastSettings _forecastSettings;
    public WeatherService(IOptions<CityCoordinates> configuration,
        IOptions<ForecastSettings> forecastSettings,
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _forecastSettings = forecastSettings.Value;
        _configuration = configuration.Value;
    }

    public Task<WeatherApiDto> GetCurrent(string city = "Moscow")
    {
        return GetData("current", city);
    }

    public Task<WeatherApiDto> GetForecast(string city = "Moscow")
    {
        return GetData("forecast", city);
    }

    private async Task<WeatherApiDto> GetData(string clientName, string city)
    {
        var coordinates = _configuration.Cities[city];
        using var client = _httpClientFactory.CreateClient(clientName);
        var response = await client.GetAsync($"?key={_forecastSettings.Key}&q={coordinates.Latitude},{coordinates.Longitude}&days=4");
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<WeatherApiDto>(content);
    }
}
