using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;
public interface IUserTasksRepository
{
    Task<KanUserTask> GetAsync(int userID, int taskID);
    Task AddAsync(KanUserTask userTask);
    Task DeleteAsync(KanUserTask userTask);
}