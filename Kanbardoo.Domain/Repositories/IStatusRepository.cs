using Kanbardoo.Domain.Entities;
using TaskStatus = Kanbardoo.Domain.Entities.TaskStatus;

namespace Kanbardoo.Domain.Repositories;

public interface ITaskStatusRepository
{
    Task<IEnumerable<TaskStatus>> GetAsync();
    Task<TaskStatus> GetAsync(int id);
    Task UpdateAsync(TaskStatus taskStatus);
    Task AddAsync(TaskStatus taskStatus);
    Task DeleteAsync(int id);
}
