namespace Kanbardoo.Application.Authorization.Requirements;

public class TaskMembershipRequirement : AuthorizationRequirement
{
    public int TaskID { get; init; }
}
