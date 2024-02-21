using System.Globalization;
using CustomProblemDetailsFactory.Contracts;
using FluentValidation;

namespace CustomProblemDetailsFactory.Validation;

public class WeatherRequestValidator : AbstractValidator<WeatherRequest>
{
    private static readonly HashSet<string> ValidCultures = 
        CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Select(culture => culture.Name)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToHashSet();

    public WeatherRequestValidator()
    {
        RuleFor(request => request.Culture)
            .Must(culture => ValidCultures.Contains(culture)).WithMessage("Culture is not a valid culture identifier.")
            .NotEmpty().WithMessage("Culture is required.");

        RuleFor(request => request.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90 degrees.");

        RuleFor(request => request.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180 degrees.");
    }
}