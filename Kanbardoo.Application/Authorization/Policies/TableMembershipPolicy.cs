using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Authorization.RequirementHandlers;
using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Http;

namespace Kanbardoo.Application.Authorization.Policies;

public class TableMembershipPolicy : ITableMembershipPolicy
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TableMembershipRequirementHandler _tableMembershipRequirementHandler;

    public TableMembershipPolicy(IHttpContextAccessor contextAccessor,
                                 TableMembershipRequirementHandler tableMembershipRequirementHandler)
    {
        _contextAccessor = contextAccessor;
        _tableMembershipRequirementHandler = tableMembershipRequirementHandler;
    }

    public async Task<Result> Authorize(int tableID)
    {
        var policy = new KanAuthorizationPolicy(_contextAccessor.HttpContext!);

        policy.AddRequirement(_tableMembershipRequirementHandler, new TableMembershipRequirement { TableID = tableID });

        return await policy.Authorize();
    }
}
