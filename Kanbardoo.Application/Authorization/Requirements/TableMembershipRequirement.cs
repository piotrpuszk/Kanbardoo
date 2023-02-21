namespace Kanbardoo.Application.Authorization.Requirements;

public class TableMembershipRequirement : AuthorizationRequirement
{
    public int TableID { get; init; }
}
