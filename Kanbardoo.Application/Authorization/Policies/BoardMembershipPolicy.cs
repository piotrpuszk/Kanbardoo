using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Authorization.RequirementHandlers;
using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Http;

namespace Kanbardoo.Application.Authorization.Policies;
public class BoardMembershipPolicy : IBoardMembershipPolicy
{
    private readonly IHttpContextAccessor _contextAccessor;

    public BoardMembershipPolicy(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public async Task<Result> Authorize(int boardID)
    {
        var policy = new KanAuthorizationPolicy(_contextAccessor.HttpContext!);
        policy.AddRequirement(new BoardMembershipRequirementHandler(), new BoardMembershipRequirement { BoardID = boardID });

        return await policy.Authorize();
    }
}
