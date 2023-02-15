using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IGetTableUseCase
{
    Task<Result<IEnumerable<Table>>> HandleAsync();
    Task<Result<Table>> HandleAsync(int id); 
}