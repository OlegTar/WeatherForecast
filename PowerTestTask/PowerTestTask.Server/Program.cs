
using Microsoft.Extensions.DependencyInjection;
using PowerTestTask.Server.Configuration;
using Configuration = PowerTestTask.Server.Configuration.CityCoordinates;
namespace PowerTestTask.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IHostEnvironment env = builder.Environment;

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

        builder.Services.Configure<CityCoordinates>(conf =>
        {
            conf.Cities = new Dictionary<string, Coordinates>();
            var cities = builder.Configuration.GetSection("Cities").GetChildren();
            foreach (var city in cities)
            {
                conf.Cities[city.Key] = new Coordinates();
                city.Bind(conf.Cities[city.Key]);
            }
            Console.WriteLine(conf);
        });

        ForecastSettings forecastSettings = new ForecastSettings();
        builder.Configuration.Bind("Forecast", forecastSettings);
        builder.Services.Configure<ForecastSettings>(forecastSettingsDi =>
        {
            forecastSettingsDi.Key = forecastSettings.Key;
            forecastSettingsDi.Current = forecastSettings.Current;
            forecastSettingsDi.Forecast = forecastSettings.Forecast;
        });

        builder.Services.AddHttpClient("current", httpClient =>
        {
            httpClient.BaseAddress = new Uri(forecastSettings.BaseUrl + forecastSettings.Current);
        });

        builder.Services.AddHttpClient("forecast", httpClient =>
        {
            httpClient.BaseAddress = new Uri(forecastSettings.BaseUrl + forecastSettings.Forecast);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
        });

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
