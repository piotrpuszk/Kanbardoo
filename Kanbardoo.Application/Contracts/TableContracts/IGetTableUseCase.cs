using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IGetTableUseCase
{
    Task<Result<IEnumerable<KanTable>>> HandleAsync();
    Task<Result<KanTable>> HandleAsync(int id); 
}