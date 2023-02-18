using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface ITableRepository
{
    Task<IEnumerable<KanTable>> GetAsync();
    Task<KanTable> GetAsync(int id);
    Task UpdateAsync(KanTable board);
    Task AddAsync(KanTable board);
    Task DeleteAsync(int id);
}
