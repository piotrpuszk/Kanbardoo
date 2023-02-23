using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Authorization.PolicyContracts;

public interface IBoardOwnershipPolicy
{
    Task<Result> AuthorizeAsync(int boardID);
}