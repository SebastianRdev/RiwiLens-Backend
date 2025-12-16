namespace src.RiwiLens.Application.Common.Auth;

public class AuthResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public string? Token { get; private set; }

    private AuthResult() { }

    public static AuthResult Ok(string token)
        => new AuthResult { Success = true, Token = token };

    public static AuthResult Fail(string message)
        => new AuthResult { Success = false, Message = message };
}
