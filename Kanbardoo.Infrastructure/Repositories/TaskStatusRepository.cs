using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class TaskStatusRepository : ITaskStatusRepository
{
    private readonly DBContext _dbContext;

    public TaskStatusRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanTaskStatus taskStatus)
    {
        await _dbContext.TaskStatuses.AddAsync(taskStatus);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.TaskStatuses
            .Where(e => e.ID == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<KanTaskStatus>> GetAsync()
    {
        return await _dbContext.TaskStatuses
            .ToListAsync();
    }

    public async Task<KanTaskStatus> GetAsync(int id)
    {
        var found = await _dbContext.TaskStatuses
            .FirstOrDefaultAsync(e => e.ID == id);

        if (found is null)
        {
            return new KanTaskStatus();
        }

        return found;
    }

    public async Task UpdateAsync(KanTaskStatus taskStatus)
    {
        _dbContext.TaskStatuses.Update(taskStatus);
    }
}
