using Microsoft.AspNetCore.Diagnostics;

namespace ComfySelf.Scheduling.API.Handlers
{
    internal sealed class ValidationExceptionHandler(
  IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not FluentValidation.ValidationException validationException)
            {
                return false;
            }

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            var context = new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Detail = "One or more validation errors occurred",
                    Status = StatusCodes.Status400BadRequest
                }
            };

            var errors = validationException.Errors.GroupBy(e => e.PropertyName).ToDictionary(
                g => g.Key.ToUpperInvariant(),
                g => g.Select(e => e.ErrorMessage).ToArray());

            context.ProblemDetails.Extensions.Add("errors", errors);
            return await problemDetailsService.TryWriteAsync(context);
        }
    }
    }
