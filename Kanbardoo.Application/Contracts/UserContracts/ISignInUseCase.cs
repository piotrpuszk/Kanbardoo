using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.UserContracts;

public interface ISignInUseCase
{
    Task<Result<KanUser>> HandleAsync(SignIn signIn);
}
