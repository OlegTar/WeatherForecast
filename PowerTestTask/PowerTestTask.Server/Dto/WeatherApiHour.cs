using System.Text.Json.Serialization;

public class WeatherApiHour
{
    [JsonPropertyName("time")]
    public string Hour { get; set; }

    [JsonPropertyName("temp_c")]
    public decimal TemperatureC { get; set; }
}