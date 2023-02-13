using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IGetTableUseCase
{
    Task<IEnumerable<Table>> HandleAsync(); 
    Task<Table> HandleAsync(int id); 
}