using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CustomProblemDetailsFactory.Controllers;

public class ExceptionController : ControllerBase
{
    private readonly ILogger<ExceptionController> _logger;

    public ExceptionController(ILogger<ExceptionController> logger)
    {
        _logger = logger;
    }
    
    [Route("/_error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment)
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;
        _logger.LogError(exception, exception?.Message ?? "Error while handling request");

        if (webHostEnvironment.IsDevelopment() || webHostEnvironment.IsStaging())
        {
            HttpContext.Items[HttpContextItemKeys.FullExceptionMessage] = GetFullExceptionMessage(exception);
        }

        return Problem(detail: exception?.Message);
    }

    private static string GetFullExceptionMessage(Exception? ex)
    {
        var message = new StringBuilder();
        while (ex != null)
        {
            message.AppendLine(ex.Message);
            message.AppendLine(ex.StackTrace);

            ex = ex.InnerException;
        }

        return message.ToString();
    }
}