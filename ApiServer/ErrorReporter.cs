using ApiServer.Protocol;

namespace ApiServer;

public static class ErrorReporter
{
    /// <summary>
    /// 외부로 예외를 알린다. (예: Slack)
    /// </summary>
    public static async Task NotifyExceptionAsync(Exception? ex)
    {
        if (ex is null) return;
        Console.WriteLine(ex);
        
        // 여기에서 외부로 알린다.
    }
    
    public static ApiResponse<T> ResponseException<T>(ApiResponse<T> response, string errorMessage = "") where T : class
    {
        response.IsSuccess = false;
        response.ErrorCode = ErrorCode.InternalServerError;
        response.ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage;
        return response;
    }
    
    public static ApiResponse ResponseException(ApiResponse response, string errorMessage = "")
    {
        response.IsSuccess = false;
        response.ErrorCode = ErrorCode.InternalServerError;
        response.ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : errorMessage;
        return response;
    }
}