using CustomProblemDetailsFactory.Controllers.Domain;

namespace CustomProblemDetailsFactory.ErrorDefinitions;
using ErrorOr;

public static class WeatherErrors
{
     public static Error WeatherNotFoundForDate(DateTimeOffset requestDateTimeOffset) => Error.NotFound(
          $"{nameof(WeatherNotFoundForDate)}",
          $"The weather for the date {requestDateTimeOffset} not found",
          new Dictionary<string, object>
          {
              [nameof(requestDateTimeOffset)] = requestDateTimeOffset
          });
}