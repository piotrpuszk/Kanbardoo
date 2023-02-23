using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Authorization.RequirementHandlers;
using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Http;

namespace Kanbardoo.Application.Authorization.Policies;

public class BoardOwnershipPolicy : IBoardOwnershipPolicy
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly BoardOwnershipRequirementHandler _boardOwnershipRequirementHandler;

    public BoardOwnershipPolicy(IHttpContextAccessor contextAccessor,
                                 BoardOwnershipRequirementHandler boardOwnershipRequirementHandler)
    {
        _contextAccessor = contextAccessor;
        _boardOwnershipRequirementHandler = boardOwnershipRequirementHandler;
    }

    public async Task<Result> AuthorizeAsync(int boardID)
    {
        var policy = new KanAuthorizationPolicy(_contextAccessor.HttpContext!);
        policy.AddRequirement(_boardOwnershipRequirementHandler, new BoardMembershipRequirement { BoardID = boardID });

        return await policy.Authorize();
    }
}