using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.UserClaimsContracts;

public interface IRevokeClaimFromUserUseCase
{
    Task<Result> HandleAsync(KanUserClaimModel userClaimModel);
}