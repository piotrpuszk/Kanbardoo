using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.UserRolesContracts;

public interface IGrantRoleToUserUseCase
{
    Task<Result> HandleAsync(UserBoardRoleGrantModel userRoleGrantModel);
}
