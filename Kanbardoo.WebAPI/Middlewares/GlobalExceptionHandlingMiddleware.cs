using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Results;
using System.Net;

namespace Kanbardoo.WebAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(Exception e)
        {
            await context.Response.WriteAsJsonAsync(Result.ErrorResult(ErrorMessage.InternalServerError));
            context.Response.ContentType = "applciation/json";
        }
    }
}
