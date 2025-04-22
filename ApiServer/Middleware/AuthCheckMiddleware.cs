using ApiServer.Protocol;
using MemoryDB;

namespace ApiServer.Middleware;

public class AuthCheckMiddleware(RequestDelegate next)
{
    // TODO: 미들웨어가 아닌 EndpointFilter를 이용하는 방법도 생각해볼 수 있다.
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        if (string.Equals(path, "/api/account/create", StringComparison.OrdinalIgnoreCase)||
            string.Equals(path, "/api/account/login", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue("Authorization", out var token) || string.IsNullOrWhiteSpace(token))
        {
            await ErrorResponse(context, ErrorCode.AuthTokenNotExists);
            return;
        }

        if (!long.TryParse(token, out var accountId))
        {
            await ErrorResponse(context, ErrorCode.ValidationFailed);
            return;
        }

        var accountCache = context.RequestServices.GetRequiredService<IAccountCache>();
        if (!await accountCache.CheckLogin(accountId))
        {
            await ErrorResponse(context, ErrorCode.NotLoggedIn);
            return;
        }

        // 레디스를 이용한 사용자 락
        if (!await accountCache.Lock(accountId))
        {
            await ErrorResponse(context, ErrorCode.RequestInProgress);
            return;
        }
        
        // 조금 더 생각해볼 방법
        //context.Items["AccountId"] = accountId;

        try
        {
            await next(context);
        }
        finally
        {
            await accountCache.Unlock(accountId);            
        }
    }

    private static async Task ErrorResponse(HttpContext context, ErrorCode errorCode)
    {
        context.Response.StatusCode = 200;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new ApiResponse
        {
            IsSuccess = false,
            ErrorCode = ErrorCode.NotLoggedIn
        });
    }
}