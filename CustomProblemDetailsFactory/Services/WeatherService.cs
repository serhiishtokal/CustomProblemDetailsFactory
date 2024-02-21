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
        
        if(request.DateTimeOffset.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            //some unexpected exception
            throw new Exception("Weekend is not supported");
        }
        
        return new Weather(14);
    }
}