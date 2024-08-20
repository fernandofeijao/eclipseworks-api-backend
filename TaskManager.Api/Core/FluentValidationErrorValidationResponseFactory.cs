using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace TaskManager.Api
{
    public class FluentValidationErrorValidationResponseFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            return new BadRequestObjectResult(AnswerAPI<string>.BadRequest(string.Empty, "Dados inválidos", validationProblemDetails?.Errors));
        }
    }
}
