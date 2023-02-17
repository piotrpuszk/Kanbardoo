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

    public Task<int> SaveChangesAsync();
}
