using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Repositories;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Kanbardoo.Application.Authorization.RequirementHandlers;
public class BoardMembershipRequirementHandler : AuthorizationRequirementHandler<BoardMembershipRequirement>
{
    private readonly IUnitOfWork _unitOfWork;

    public BoardMembershipRequirementHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task HandleAsync(AuthorizationRequirementContext context, BoardMembershipRequirement requirement)
    {
        if (!context.User.HasClaim(e => e.Type == KanClaimName.ID))
        {
            return;
        }

        var userID = int.Parse(context.User.FindFirstValue(KanClaimName.ID)!);

        await ValidateBoardID(context, requirement, userID);
    }

    private async Task ValidateBoardID(AuthorizationRequirementContext context,
                                        BoardMembershipRequirement requirement,
                                        int userID)
    {
        var board = await _unitOfWork.UserBoardsRepository.GetAsync(userID, requirement.BoardID);

        if (board.Exists())
        {
            context.Succeed(requirement);
        }
    }
}
