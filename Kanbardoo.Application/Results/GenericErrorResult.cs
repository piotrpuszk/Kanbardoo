using System.Net;

public sealed class ErrorResult<T> : Result<T>
{
    public ErrorResult(IEnumerable<string> errors, HttpStatusCode httpStatusCode)
    {
        IsSuccess = false;
        Errors = errors;
        HttpCode = httpStatusCode;
    }
}