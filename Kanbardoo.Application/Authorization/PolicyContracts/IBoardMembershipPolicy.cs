using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Authorization.PolicyContracts;
public interface IBoardMembershipPolicy
{
    Task<Result> AuthorizeAsync(int boardID);
}
