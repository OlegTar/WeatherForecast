using System.Text.Json.Serialization;

namespace PowerTestTask.Server.Dto;

public class WeatherApiDto
{
    [JsonPropertyName("current")]
    public WeatherApiCurrentDto Current { get; set; }
}
