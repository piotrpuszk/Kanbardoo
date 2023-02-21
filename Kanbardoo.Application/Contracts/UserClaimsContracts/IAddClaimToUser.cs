using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.UserClaimsContracts;
public interface IAddClaimToUserUseCase
{
    Task<Result> HandleAsync(KanUserClaimModel userClaimModel);
}
