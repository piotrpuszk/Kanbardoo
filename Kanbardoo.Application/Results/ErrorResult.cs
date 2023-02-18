using System.Net;

namespace Kanbardoo.Application.Results;

public sealed class ErrorResult : Result
{
    public ErrorResult(IEnumerable<string> errors, HttpStatusCode httpStatusCode)
    {
        IsSuccess = false;
        Errors = errors;
        HttpCode= httpStatusCode;
    }
}