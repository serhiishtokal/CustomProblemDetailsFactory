using CustomProblemDetailsFactory.Contracts;
using CustomProblemDetailsFactory.Controllers.Domain;
using CustomProblemDetailsFactory.ErrorDefinitions;
using ErrorOr;

namespace CustomProblemDetailsFactory.Services;

public class WeatherService
{
    public static async Task<ErrorOr<Weather>> GetWeatherAsync(WeatherRequest request, CancellationToken ct = default)
    {
        await Task.Delay(1000, ct);
        if (request.DateTimeOffset < DateTimeOffset.Now.AddYears(-50) || request.DateTimeOffset > DateTimeOffset.Now.AddDays(7))
        {
            return WeatherErrors.WeatherNotFoundForDate(request.DateTimeOffset);
        }
        
        return new Weather(14);
    }
}