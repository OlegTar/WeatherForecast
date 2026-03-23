using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PowerTestTask.Server.Configuration;
using PowerTestTask.Server.Dto;
using System.Text.Json;

namespace PowerTestTask.Server.Controllers
{
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
        public async Task<CurrentDto> Get([FromQuery]string city = "Moscow")
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
    }
}
