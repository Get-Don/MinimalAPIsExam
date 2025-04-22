namespace ApiServer.Protocol;

public enum ErrorCode
{
    Ok,
    
    // Error
    InternalServerError = 1000,
    ValidationFailed,
    RequestInProgress,
    
    // Account Error
    EmailAlreadyExists = 2000,
    AccountNotExist,
    IncorrectPassword,
    NotLoggedIn,
    AuthTokenNotExists,
    
    // Money Error
    
    // Stat Error
}

public class ApiResponse<T> where T : class
{
    private ErrorCode _errorCode = Protocol.ErrorCode.Ok;

    public ErrorCode ErrorCode
    {
        get => _errorCode;
        set
        {
            _errorCode = value;
            ErrorCodeName = value.ToString();
        }
    }
    
    public string ErrorCodeName { get; private set; } = nameof(ErrorCode.Ok);
    public string? ErrorMessage { get; set; }
    public bool IsSuccess { get; set; } = true;
    
    public T? Result { get; set; }
}

public class ApiResponse : ApiResponse<object>;