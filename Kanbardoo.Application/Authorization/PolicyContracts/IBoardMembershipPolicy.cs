using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Authorization.PolicyContracts;
public interface IBoardMembershipPolicy
{
    Task<Result> Authorize(int boardID);
}
