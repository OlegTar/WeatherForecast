namespace PowerTestTask.Server.Dto;

public class DaysForecastsDto
{
    public decimal? TomorrowTemperature { get; set; }
    public decimal? DayAfterTomorrowTemperature { get; set; }
    public decimal? TwoDaysAfterTomorrowTemperature { get; set; }
}
