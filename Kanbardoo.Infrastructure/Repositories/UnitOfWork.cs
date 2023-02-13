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

    public UnitOfWork(IBoardRepository boardRepository,
                      DBContext dbContext)
    {
        BoardRepository = boardRepository;
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
