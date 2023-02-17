using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class TaskRepository : ITaskRepository
{
    private readonly DBContext _dbContext;

    public TaskRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanTask task)
    {
        task.Status = null!;
        task.Assignee = null!;
        task.Table = null!;
        await _dbContext.Tasks.AddAsync(task);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.Tasks
            .Where(e => e.ID == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<KanTask>> GetAsync()
    {
        return await _dbContext.Tasks
            .ToListAsync();
    }

    public async Task<KanTask> GetAsync(int id)
    {
        var found = await _dbContext.Tasks
            .Include(e => e.Assignee)
            .Include(e => e.Status)
            .FirstOrDefaultAsync(e => e.ID == id);

        if (found is null)
        {
            return new KanTask();
        }

        return found;
    }

    public async Task UpdateAsync(KanTask task)
    {
        task.Status = null!;
        task.Assignee = null!;
        task.Table = null!;
        _dbContext.Tasks.Update(task);
    }
}
