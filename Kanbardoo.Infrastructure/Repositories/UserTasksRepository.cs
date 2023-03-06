using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class UserTasksRepository : IUserTasksRepository
{
    private readonly DBContext _dbContext;

    public UserTasksRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanUserTask userTask)
    {
        userTask.Task = null!;
        userTask.User = null!;
        userTask.GeneratePrimaryKey();
        await _dbContext.UserTasks.AddAsync(userTask);
    }

    public async Task DeleteAsync(KanUserTask userTask)
    {
        await _dbContext.UserTasks.Where(e => e.UserID == userTask.UserID && e.TaskID == userTask.TaskID).ExecuteDeleteAsync();
    }

    public async Task<KanUserTask> GetAsync(int userID, int taskID)
    {
        var result = await _dbContext.UserTasks.FirstOrDefaultAsync(e => e.UserID == userID && e.TaskID == taskID);

        if (result is null)
        {
            return new KanUserTask();
        }

        return result;
    }
}
