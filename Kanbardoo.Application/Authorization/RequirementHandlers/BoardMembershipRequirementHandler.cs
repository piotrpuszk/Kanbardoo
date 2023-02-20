using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Domain.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Kanbardoo.Application.Authorization.RequirementHandlers;
public class BoardMembershipRequirementHandler : AuthorizationRequirementHandler<BoardMembershipRequirement>
{
    public override Task HandleAsync(AuthorizationRequirementContext context, BoardMembershipRequirement requirement)
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

    private Task ValidateBoardID(AuthorizationRequirementContext context,
                                        BoardMembershipRequirement requirement,
                                        IEnumerable<int>? boardMembershipIDs)
    {
        if (boardMembershipIDs!.Contains(requirement.BoardID))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
