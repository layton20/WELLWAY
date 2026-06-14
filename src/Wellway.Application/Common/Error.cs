namespace Wellway.Application.Common;

public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Validation(string message)   => new("Validation",   message);
    public static Error NotFound(string message)     => new("NotFound",     message);
    public static Error Unauthorised(string message) => new("Unauthorised", message);
    public static Error Conflict(string message)     => new("Conflict",     message);
    public static Error Unexpected(string message)   => new("Unexpected",   message);
}
