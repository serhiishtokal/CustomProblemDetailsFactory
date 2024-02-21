using CustomProblemDetailsFactory.Contracts;
using CustomProblemDetailsFactory.Filters;
using CustomProblemDetailsFactory.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomProblemDetailsFactory.Controllers;

[Route("api/[controller]")]
public class WeatherController : ApiController
{
    private readonly WeatherService _weatherService;

    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }
    
    [HttpPost("Sample1")]
    [ValidateWithFluentValidation<WeatherRequest>]
    public async Task<IActionResult> Sample1(WeatherRequest request,
        CancellationToken ct = default)
    {
        var weatherResult = await WeatherService.GetWeatherAsync(request, ct);

        return weatherResult.Match(Ok,Problem);
    }
}