using PowerTestTask.Server.Dto;

namespace DataLayer
{
    public interface IWeatherService
    {
        Task<WeatherApiDto> GetCurrent(string city = "Moscow");
        Task<WeatherApiDto> GetForecast(string city = "Moscow");
    }
}