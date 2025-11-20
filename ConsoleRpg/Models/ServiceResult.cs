namespace ConsoleRpg.Models;

/// <summary>
/// Represents the result of a service operation
/// Follows SOLID principles by decoupling service logic from UI concerns
/// </summary>
public class ServiceResult
{
    public bool Success { get; }
    public string Message { get; }
    public string DetailedOutput { get; }

    public ServiceResult(bool success, string message, string detailedOutput = null)
    {
        Success = success;
        Message = message;
        DetailedOutput = detailedOutput ?? message;
    }

    /// <summary>
    /// Creates a successful result with optional message and detailed output
    /// </summary>
    public static ServiceResult Ok(string message = "", string detailedOutput = null)
    {
        return new ServiceResult(true, message, detailedOutput);
    }

    /// <summary>
    /// Creates a failed result with error message
    /// </summary>
    public static ServiceResult Fail(string message, string detailedOutput = null)
    {
        return new ServiceResult(false, message, detailedOutput);
    }
}

/// <summary>
/// Generic result with a return value
/// </summary>
public class ServiceResult<T> : ServiceResult
{
    public T Value { get; }

    private ServiceResult(bool success, T value, string message, string detailedOutput = null)
        : base(success, message, detailedOutput)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    public static ServiceResult<T> Ok(T value, string message = "", string detailedOutput = null)
    {
        return new ServiceResult<T>(true, value, message, detailedOutput);
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    public new static ServiceResult<T> Fail(string message, string detailedOutput = null)
    {
        return new ServiceResult<T>(false, default, message, detailedOutput);
    }
}
