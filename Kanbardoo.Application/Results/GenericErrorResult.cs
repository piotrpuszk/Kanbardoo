using Kanbardoo.Application.Results;

public sealed class ErrorResult<T> : Result<T>
{
    public ErrorResult(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }
}