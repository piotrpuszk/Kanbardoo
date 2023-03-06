using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

public class UserBoardsRepository : IUserBoardsRepository
{
    private readonly DBContext _dbContext;

    public UserBoardsRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanUserBoard userBoard)
    {
        userBoard.User = null!;
        userBoard.Board = null!;
        userBoard.GeneratePrimaryKey();
        await _dbContext.UserBoards.AddAsync(userBoard);
    }

    public async Task DeleteAsync(KanUserBoard userBoard)
    {
        await _dbContext.UserBoards.Where(e => e.UserID == userBoard.UserID && e.BoardID == userBoard.BoardID).ExecuteDeleteAsync();
    }

    public async Task<KanUserBoard> GetAsync(int userID, int boardID)
    {
        var result = await _dbContext.UserBoards.FirstOrDefaultAsync(e => e.UserID == userID && e.BoardID == boardID);

        if (result is null)
        {
            return new KanUserBoard();
        }

        return result;
    }
}