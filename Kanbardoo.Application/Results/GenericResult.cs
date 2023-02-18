using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Content { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public HttpStatusCode HttpCode { get; set; }

    protected Result()
    {

    }

    public IActionResult GetActionResult()
    {
        switch (HttpCode)
        {
            case HttpStatusCode.OK:
                return new OkObjectResult(this);
            case HttpStatusCode.BadRequest:
                return new BadRequestObjectResult(this);
            default:
                var result = new ObjectResult(this);
                result.StatusCode = (int)HttpCode;
                return result;
        }
    }

    public static Result<T> SuccessResult(T content)
    {
        return new SuccessResult<T>(content);
    }

    public static Result<T> ErrorResult(IEnumerable<string> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new ErrorResult<T>(errors, httpStatusCode);
    }

    public static Result<T> ErrorResult(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return ErrorResult(new List<string>() { error }, httpStatusCode);
    }
}