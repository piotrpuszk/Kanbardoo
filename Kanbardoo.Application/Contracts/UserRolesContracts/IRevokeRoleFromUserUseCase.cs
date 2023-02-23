using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.UserRolesContracts;

public interface IRevokeRoleFromUserUseCase
{
    Task<Result> HandleAsync(UserRoleRevokeModel userRoleRevokeModel);
}
