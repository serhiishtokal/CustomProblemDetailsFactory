namespace CustomProblemDetailsFactory.Contracts;

public record WeatherRequest(DateTimeOffset DateTimeOffset, float Latitude, float Longitude, string Culture = "en");