using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<KanTask>> GetAsync();
    Task<KanTask> GetAsync(int id);
    Task UpdateAsync(KanTask task);
    Task AddAsync(KanTask task);
    Task DeleteAsync(int id);
}
