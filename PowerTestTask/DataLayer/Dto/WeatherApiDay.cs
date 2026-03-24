using System.Text.Json.Serialization;

public class WeatherApiDay
{
    [JsonPropertyName("avgtemp_c")]
    public decimal AvgTemperature { get; set; }
}