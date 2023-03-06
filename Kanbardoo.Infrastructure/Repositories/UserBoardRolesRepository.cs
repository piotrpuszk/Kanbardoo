using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

public class UserBoardRolesRepository : IUserBoardRolesRepository
{
    private readonly DBContext _dbContext;

    public UserBoardRolesRepository(DBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task AddAsync(KanUserBoardRole userBoardRole)
    {
        userBoardRole.Role = null!;
        userBoardRole.User = null!;
        userBoardRole.GeneratePrimaryKey();
        await _dbContext.UserBoardsRoles
            .AddAsync(userBoardRole);
    }

    public async Task DeleteAsync(KanUserBoardRole userBoardRole)
    {
        await _dbContext.UserBoardsRoles
            .Where(e => e.UserID == userBoardRole.UserID && e.RoleID == userBoardRole.RoleID)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteAsync(int userID, int boardID)
    {
        await _dbContext.UserBoardsRoles
            .Where(e => e.UserID == userID && e.BoardID == boardID)
            .ExecuteDeleteAsync();
    }

    public async Task<KanUserBoardRole> GetAsync(KanUserBoardRole userBoardRole)
    {
        var result = await _dbContext.UserBoardsRoles
            .FirstOrDefaultAsync(e => e.UserID == userBoardRole.UserID && e.RoleID == userBoardRole.RoleID);

        if (result == null)
        {
            return new KanUserBoardRole();
        }

        return result;
    }

    public async Task<IEnumerable<KanUserBoardRole>> GetAsync(int userID, int boardID)
    {
        return await _dbContext.UserBoardsRoles
            .Where(e => e.UserID == userID && e.BoardID == boardID)
            .ToListAsync();
    }
}