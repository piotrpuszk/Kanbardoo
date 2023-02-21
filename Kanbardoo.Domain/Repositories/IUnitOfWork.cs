using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Domain.Repositories;
public interface IUnitOfWork
{
    public IBoardRepository BoardRepository { get; init; }
    public ITableRepository TableRepository { get; init; }
    public ITaskRepository TaskRepository { get; init; }
    public ITaskStatusRepository TaskStatusRepository { get; init; }
    public IUserRepository UserRepository { get; init; }
    public IUserClaimsRepository UserClaimsRepository { get; init; }
    public IClaimRepository ClaimRepository { get; init; }
    public IUserBoardsRepository UserBoardsRepository { get; init; }
    public IUserTablesRepository UserTablesRepository { get; init; }
    public IUserTasksRepository UserTasksRepository { get; init; }

    public Task<int> SaveChangesAsync();
}
