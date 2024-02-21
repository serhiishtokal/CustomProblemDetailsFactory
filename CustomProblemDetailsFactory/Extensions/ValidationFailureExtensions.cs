using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CustomProblemDetailsFactory.Extensions;

public static class ValidationFailureExtensions
{
    public static ModelStateDictionary ToModelStateDictionary(this List<ValidationFailure>? validationFailures)
    {
        var modelStateDictionary = new ModelStateDictionary();
        if (validationFailures is null)
        {
            return modelStateDictionary;
        }

        foreach (var validationFailure in validationFailures)
        {
            modelStateDictionary.AddModelError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage);
        }

        return modelStateDictionary;
    }
}