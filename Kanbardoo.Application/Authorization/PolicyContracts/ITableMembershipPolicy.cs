using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Authorization.PolicyContracts;

public interface ITableMembershipPolicy
{
    Task<Result> AuthorizeAsync(int tableID);
}

