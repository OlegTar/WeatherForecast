using System.Text.Json.Serialization;

public class WeatherApiForecastDayDto
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("day")]
    public WeatherApiDay Day { get; set; }

    [JsonPropertyName("hour")]
    public ICollection<WeatherApiHour> Hours { get; set; }
}