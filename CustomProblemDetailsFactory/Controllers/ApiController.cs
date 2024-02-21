using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ErrorOr    ;

namespace CustomProblemDetailsFactory.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        
        // Problem details factory can be used to create a problem details object
        // var problemDetails = ProblemDetailsFactory.CreateProblemDetails(HttpContext, statusCode, firstError.Description);
        // return new ObjectResult(problemDetails)
        // {
        //     StatusCode = problemDetails.Status
        // };
        
        // Or we can use the Problem method to create a object result
        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}