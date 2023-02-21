using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Repositories;
using System.Security.Claims;

namespace Kanbardoo.Application.Authorization.RequirementHandlers;

public class TaskMembershipRequirementHandler : AuthorizationRequirementHandler<TaskMembershipRequirement>
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskMembershipRequirementHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task HandleAsync(AuthorizationRequirementContext context, TaskMembershipRequirement requirement)
    {
        if (!context.User.HasClaim(e => e.Type == KanClaimName.ID))
        {
            return;
        }

        var userID = int.Parse(context.User.FindFirstValue(KanClaimName.ID)!);

        await ValidateBoardID(context, requirement, userID);
    }

    private async Task ValidateBoardID(AuthorizationRequirementContext context,
                                        TaskMembershipRequirement requirement,
                                        int userID)
    {
        var task = await _unitOfWork.UserTasksRepository.GetAsync(userID, requirement.TaskID);

        if (task.Exists())
        {
            context.Succeed(requirement);
        }
    }
}
