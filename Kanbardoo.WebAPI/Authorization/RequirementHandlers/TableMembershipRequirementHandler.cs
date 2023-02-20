using Kanbardoo.Domain.Authorization;
using Kanbardoo.WebAPI.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Kanbardoo.WebAPI.Authorization.RequirementHandlers;

public class TableMembershipRequirementHandler : AuthorizationHandler<TableMembershipRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TableMembershipRequirement requirement)
    {
        if (!context.User.HasClaim(e => e.Type == KanClaimName.Member))
        {
            return Task.CompletedTask;
        }

        var boardMembershipIDsJson = context.User.FindFirstValue(KanClaimName.Member);

        if (boardMembershipIDsJson is null)
        {
            return Task.CompletedTask;
        }

        var boardMembershipIDs = JsonConvert.DeserializeObject<IEnumerable<int>>(boardMembershipIDsJson);

        return Validate(context, boardMembershipIDs);
    }

    private Task Validate(AuthorizationHandlerContext context, IEnumerable<int>? boardMembershipIDs)
    {
        if (context.Resource is not HttpContext httpContext)
        {
            return Task.CompletedTask;
        }

        foreach (var routePair in httpContext.GetRouteData().Values)
        {

        }

        return Task.CompletedTask;
    }
}
