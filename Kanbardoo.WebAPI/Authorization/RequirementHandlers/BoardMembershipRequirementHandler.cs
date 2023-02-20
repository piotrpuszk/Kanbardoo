using Kanbardoo.Domain.Authorization;
using Kanbardoo.WebAPI.Authorization.Requirements;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Kanbardoo.WebAPI.Authorization.RequirementHandlers;
public class BoardMembershipRequirementHandler : AuthorizationHandler<BoardMembershipRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BoardMembershipRequirement requirement)
    {
        if (!context.User.HasClaim(e => e.Type == KanClaimName.Member))
        {
            return Task.CompletedTask;
        }

        var boardMembershipIDsJSON = context.User.FindFirstValue(KanClaimName.Member);

        if (boardMembershipIDsJSON is null)
        {
            return Task.CompletedTask;
        }

        var boardMembershipIDs = JsonConvert.DeserializeObject<IEnumerable<int>>(boardMembershipIDsJSON);

        return ValidateBoardID(context, requirement, boardMembershipIDs);
    }

    private Task ValidateBoardID(AuthorizationHandlerContext context,
                                        BoardMembershipRequirement requirement,
                                        IEnumerable<int>? boardMembershipIDs)
    {
        if (context.Resource is not HttpContext httpContext)
        {
            return Task.CompletedTask;
        }

        int boardID = default;
        foreach (var routePair in httpContext.GetRouteData().Values)
        {
            boardID = routePair switch
            {
                { Key: var key, Value: var value } when key == "id" => int.Parse(routePair.Value!.ToString()!),
                { Key: var key, Value: var value } when value is KanBoardDTO board => board.ID,
                _ => default,
            };

            if (boardID != default)
            {
                break;
            }
        }

        if (boardID != default && boardMembershipIDs!.Contains(boardID))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
