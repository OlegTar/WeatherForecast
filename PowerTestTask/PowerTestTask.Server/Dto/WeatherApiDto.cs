using System.Text.Json.Serialization;

namespace PowerTestTask.Server.Dto;

public class WeatherApiDto
{
    [JsonPropertyName("current")]
    public WeatherApiCurrentDto Current { get; set; }

    [JsonPropertyName("forecast")]
    public WeatherApiForecastDto Forecast { get; set; }
}
