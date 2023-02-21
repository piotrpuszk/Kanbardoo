using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Authorization.PolicyContracts;

public interface ITaskMembershipPolicy
{
    Task<Result> Authorize(int taskID);
}

