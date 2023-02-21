using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Authorization.RequirementHandlers;
using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Application.Results;
using Microsoft.AspNetCore.Http;

namespace Kanbardoo.Application.Authorization.Policies;

public class TaskMembershipPolicy : ITaskMembershipPolicy
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TaskMembershipRequirementHandler __taskMembershipRequirementHandler;

    public TaskMembershipPolicy(IHttpContextAccessor contextAccessor,
                                 TaskMembershipRequirementHandler taskMembershipRequirementHandler)
    {
        _contextAccessor = contextAccessor;
        __taskMembershipRequirementHandler = taskMembershipRequirementHandler;
    }

    public async Task<Result> AuthorizeAsync(int taskID)
    {
        var policy = new KanAuthorizationPolicy(_contextAccessor.HttpContext!);

        policy.AddRequirement(__taskMembershipRequirementHandler, new TaskMembershipRequirement { TaskID = taskID });

        return await policy.Authorize();
    }
}
