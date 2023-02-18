using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface ITaskStatusRepository
{
    Task<IEnumerable<KanTaskStatus>> GetAsync();
    Task<KanTaskStatus> GetAsync(int id);
    Task UpdateAsync(KanTaskStatus taskStatus);
    Task AddAsync(KanTaskStatus taskStatus);
    Task DeleteAsync(int id);
}
