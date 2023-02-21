using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Repositories;
using System.Security.Claims;

namespace Kanbardoo.Application.Authorization.RequirementHandlers;

public class TableMembershipRequirementHandler : AuthorizationRequirementHandler<TableMembershipRequirement>
{
    private readonly IUnitOfWork _unitOfWork;

    public TableMembershipRequirementHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task HandleAsync(AuthorizationRequirementContext context, TableMembershipRequirement requirement)
    {
        if (!context.User.HasClaim(e => e.Type == KanClaimName.ID))
        {
            return;
        }

        var userID = int.Parse(context.User.FindFirstValue(KanClaimName.ID)!);

        await ValidateBoardID(context, requirement, userID);
    }

    private async Task ValidateBoardID(AuthorizationRequirementContext context,
                                        TableMembershipRequirement requirement,
                                        int userID)
    {
        var table = await _unitOfWork.UserTablesRepository.GetAsync(userID, requirement.TableID);

        if (table.Exists())
        {
            context.Succeed(requirement);
        }
    }
}
