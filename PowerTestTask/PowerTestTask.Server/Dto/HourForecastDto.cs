namespace PowerTestTask.Server.Dto;

public class HourForecastDto
{
    public ICollection<HourDto> Today { get; set; }
    public ICollection<HourDto> Tomorrow { get; set; }
}
