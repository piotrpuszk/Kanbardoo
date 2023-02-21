using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

public class UserTablesRepository : IUserTablesRepository
{
    private readonly DBContext _dbContext;

    public UserTablesRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanUserTable userTable)
    {
        userTable.Table = null!;
        userTable.User= null!;
        await _dbContext.UserTables.AddAsync(userTable);
    }

    public async Task DeleteAsync(KanUserTable userTable)
    {
        await _dbContext.UserTables.Where(e => e.UserID == userTable.UserID && e.TableID == userTable.TableID).ExecuteDeleteAsync();
    }

    public async Task<KanUserTable> GetAsync(int userID, int tableID)
    {
        var result = await _dbContext.UserTables.FirstOrDefaultAsync(e => e.UserID == userID && e.TableID == tableID);

        if (result is null)
        {
            return new KanUserTable();
        }

        return result;
    }
}
