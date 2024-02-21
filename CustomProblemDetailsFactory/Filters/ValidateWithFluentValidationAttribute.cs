using CustomProblemDetailsFactory.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CustomProblemDetailsFactory.Filters;

public class ValidateWithFluentValidationAttribute<TValidationModel> : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var modelToValidate = context.ActionArguments
            .Where(x => x.Value is TValidationModel)
            .Select(x => (TValidationModel)x.Value!)
            .FirstOrDefault();

        var result = await ValidateAsync(modelToValidate, () => GetValidator(context));
        if (result?.Count > 0)
        {
            var problemDetailsFactory = context.HttpContext.RequestServices.GetService<ProblemDetailsFactory>()!;
            var modelStateDictionary = result.ToModelStateDictionary();
            var validationProblemDetails =
                problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, modelStateDictionary);

            context.Result = new BadRequestObjectResult(validationProblemDetails);
            return;
        }

        await next();
    }

    private static IValidator<TValidationModel>? GetValidator(ActionContext context)
    {
        return context.HttpContext.RequestServices.GetService<IValidator<TValidationModel>>();
    }

    private static async Task<List<ValidationFailure>?> ValidateAsync(TValidationModel? modelToValidate,
        Func<IValidator<TValidationModel>?> validatorGetter)
    {
        if (modelToValidate is null)
        {
            return null;
        }

        var validator = validatorGetter();
        if (validator is null)
        {
            return null;
        }

        var validationResult = await validator.ValidateAsync(modelToValidate);
        return validationResult.IsValid ? null : validationResult.Errors;
    }
}