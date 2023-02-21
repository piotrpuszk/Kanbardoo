using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Kanbardoo.Application.Authorization;

public class KanAuthorizationPolicy
{
    private readonly AuthorizationRequirementContext _context;
    private List<(dynamic handler, dynamic requirement)> _handlers
        = new();

    public KanAuthorizationPolicy(HttpContext httpContext)
    {
        _context = new AuthorizationRequirementContext(httpContext);
    }

    public KanAuthorizationPolicy AddRequirement<T>(AuthorizationRequirementHandler<T> handler,
                                              T requirement) where T : AuthorizationRequirement
    {
        _handlers.Add((handler, requirement));
        return this;
    }

    public async Task<Result> Authorize()
    {
        var tasks = new List<Task>();
        foreach (var pair in _handlers)
        {
            tasks.Add(pair.handler.HandleAsync(_context, pair.requirement));
        }

        await Task.WhenAll(tasks);

        if (_context.HasSucceeded())
        {
            return Result.SuccessResult();
        }
        else
        {
            return Result.ErrorResult(ErrorMessage.Unauthorized, HttpStatusCode.Unauthorized);
        }
    }
}
