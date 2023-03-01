using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.UserContracts;

public interface IGetUsersUseCase
{
    Task<Result<IEnumerable<KanUser>>> HandleAsync(string query);
}