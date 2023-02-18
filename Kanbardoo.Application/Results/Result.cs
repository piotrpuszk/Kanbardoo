using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Kanbardoo.Application.Results;

public class Result
{
    public bool IsSuccess { get; set; }
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

    public static Result SuccessResult()
    {
        return new SuccessResult();
    }

    public static Result ErrorResult(IEnumerable<string> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new ErrorResult(errors, httpStatusCode);
    }

    public static Result ErrorResult(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        return new ErrorResult(new List<string> { error }, httpStatusCode);
    }
}