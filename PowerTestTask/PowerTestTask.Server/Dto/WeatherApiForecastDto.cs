using System.Text.Json.Serialization;

public class WeatherApiForecastDto
{
    [JsonPropertyName("forecastday")]
    public ICollection<WeatherApiForecastDayDto> ForecastDay { get; set; }
}
