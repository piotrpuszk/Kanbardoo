using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.UserClaimsContracts;
public interface IAddClaimToUserUseCase
{
    Task<Result> HandleAsync(KanUserClaim userClaim);
}
