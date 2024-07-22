using Application.Common.Responses;
using FluentValidation;

namespace WebApi.Common.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argToValidate = context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) as T;
        
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(argToValidate!);
            if (!validationResult.IsValid)
            {
                var errorResponse = new ErrorResponse<T>("One or more validation errors occured.",
                    validationResult.ToDictionary());
                
                return Results.BadRequest(errorResponse);
            }
        }
        
        return await next.Invoke(context);
    }
}