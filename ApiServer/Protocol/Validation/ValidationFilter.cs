using ApiServer.Protocol;
using FluentValidation;

namespace ApiServer.Protocol.Validation;

public class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is null)
        {
            return await next(context);
        }

        var obj = context.Arguments.OfType<T>().FirstOrDefault();
        if (obj is null)
        {
            return new ApiResponse
            {
                ErrorCode = ErrorCode.ValidationFailed,
                ErrorMessage = "Invalid body",
                IsSuccess = false
            };
        }
        
        var validationResult = await validator.ValidateAsync(obj);
        if (!validationResult.IsValid)
        {
            return new ApiResponse
            {
                ErrorCode = ErrorCode.ValidationFailed,
                IsSuccess = false,
                Result = validationResult.ToDictionary()
            };
        }
        
        return await next(context);
    }
}