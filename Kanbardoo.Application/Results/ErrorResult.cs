namespace Kanbardoo.Application.Results;

public sealed class ErrorResult : Result
{
    public ErrorResult(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }
}