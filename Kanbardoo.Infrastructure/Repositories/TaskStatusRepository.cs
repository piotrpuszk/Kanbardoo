using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Kanbardoo.Domain.Entities.TaskStatus;

namespace Kanbardoo.Infrastructure.Repositories;
public class TaskStatusRepository : ITaskStatusRepository
{
    private readonly DBContext _dbContext;

    public TaskStatusRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TaskStatus taskStatus)
    {
        await _dbContext.TaskStatuses.AddAsync(taskStatus);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.TaskStatuses
            .Where(e => e.ID == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<TaskStatus>> GetAsync()
    {
        return await _dbContext.TaskStatuses
            .ToListAsync();
    }

    public async Task<TaskStatus> GetAsync(int id)
    {
        var found = await _dbContext.TaskStatuses
            .FirstOrDefaultAsync(e => e.ID == id);

        if (found is null)
        {
            return new TaskStatus();
        }

        return found;
    }

    public async Task UpdateAsync(TaskStatus taskStatus)
    {
        _dbContext.TaskStatuses.Update(taskStatus);
    }
}
