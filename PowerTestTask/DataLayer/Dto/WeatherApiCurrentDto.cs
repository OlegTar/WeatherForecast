using System.Text.Json.Serialization;

namespace PowerTestTask.Server.Dto;

public class WeatherApiCurrentDto
{
    [JsonPropertyName("temp_c")]
    public decimal Temperature { get; set; }
}
