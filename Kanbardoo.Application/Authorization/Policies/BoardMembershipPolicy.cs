using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Authorization.RequirementHandlers;
using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Http;

namespace Kanbardoo.Application.Authorization.Policies;
public class BoardMembershipPolicy : IBoardMembershipPolicy
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly BoardMembershipRequirementHandler _boardMembershipRequirementHandler;
    private readonly BoardOwnershipRequirementHandler _boardOwnershipRequirementHandler;

    public BoardMembershipPolicy(IHttpContextAccessor contextAccessor,
                                 BoardMembershipRequirementHandler boardMembershipRequirementHandler,
                                 BoardOwnershipRequirementHandler boardOwnershipRequirementHandler)
    {
        _contextAccessor = contextAccessor;
        _boardMembershipRequirementHandler = boardMembershipRequirementHandler;
        _boardOwnershipRequirementHandler = boardOwnershipRequirementHandler;
    }

    public async Task<Result> AuthorizeAsync(int boardID)
    {
        var policy = new KanAuthorizationPolicy(_contextAccessor.HttpContext!);
        policy.AddRequirement(_boardMembershipRequirementHandler, new BoardMembershipRequirement { BoardID = boardID })
            .AddRequirement(_boardOwnershipRequirementHandler, new BoardMembershipRequirement { BoardID = boardID });

        return await policy.Authorize();
    }
}
