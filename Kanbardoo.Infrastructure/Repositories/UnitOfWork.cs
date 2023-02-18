using Kanbardoo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly DBContext _dbContext;

    public IBoardRepository BoardRepository { get; init; }
    public ITableRepository TableRepository { get; init; }
    public ITaskRepository TaskRepository { get; init; }
    public ITaskStatusRepository TaskStatusRepository { get; init; }
    public IUserRepository UserRepository { get; init; }
    public IUserClaimsRepository UserClaimsRepository { get; init; }
    public IClaimRepository ClaimRepository { get; init; }

    public UnitOfWork(DBContext dbContext,
                      IBoardRepository boardRepository,
                      ITableRepository tableRepository,
                      ITaskRepository taskRepository,
                      ITaskStatusRepository taskStatusRepository,
                      IUserRepository userRepository,
                      IUserClaimsRepository userClaimsRepository,
                      IClaimRepository claimRepository)
    {
        BoardRepository = boardRepository;
        TableRepository = tableRepository;
        _dbContext = dbContext;
        TaskRepository = taskRepository;
        TaskStatusRepository = taskStatusRepository;
        UserRepository = userRepository;
        UserClaimsRepository = userClaimsRepository;
        ClaimRepository = claimRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
