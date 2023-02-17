using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface ITableRepository
{
    Task<IEnumerable<Table>> GetAsync();
    Task<Table> GetAsync(int id);
    Task UpdateAsync(Table board);
    Task AddAsync(Table board);
    Task DeleteAsync(int id);
}
