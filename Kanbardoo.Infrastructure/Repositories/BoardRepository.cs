using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class BoardRepository : IBoardRepository
{
    private readonly DBContext _dbContext;

    public BoardRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanBoard board)
    {
        board.Owner = null!;
        board.Status = null!;
        await _dbContext.Boards.AddAsync(board);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.Boards
            .Where(e => e.ID == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<KanBoard>> GetAsync()
    {
        return await _dbContext.Boards
            .Include(e => e.Owner)
            .Include(e => e.Status)
            .ToListAsync();
    }

    public async Task<IEnumerable<KanBoard>> GetAsync(KanBoardFilters boardFilters)
    {
        IQueryable<KanBoard> query;
        switch (boardFilters.RoleID)
        {
            case KanRoleID.Member:
                query = _dbContext.Boards
                    .Where(e => e.OwnerID != boardFilters.OwnerID)
                    .Where(e => _dbContext.UserBoardsRoles
                                                .Where(e => e.UserID == boardFilters.OwnerID).Select(e => e.BoardID)
                                                .Contains(e.ID))
                    .Include(e => e.Owner)
                    .Include(e => e.Status)
                    .AsQueryable();
                break;
            default:
                query = _dbContext.Boards
                    .Where(e => e.OwnerID == boardFilters.OwnerID)
                    .Include(e => e.Owner)
                    .Include(e => e.Status)
                    .AsQueryable();
                break;
        }



        var boardName = boardFilters.BoardName;
        if (!string.IsNullOrWhiteSpace(boardName))
        {
            query = query.Where(e => e.Name.ToLower().Contains(boardName.ToLower()));
        }

        query = query.OrderByClauses(boardFilters.OrderByClauses);

        return await query.ToListAsync();
    }

    public async Task<KanBoard> GetAsync(int id)
    {
        var found = await _dbContext.Boards
            .Include(e => e.Owner)
            .Include(e => e.Status)
            .Include(e => e.Tables.OrderBy(e => e.Priority))
            .ThenInclude(e => e.Tasks)
            .ThenInclude(e => e.Status)
            .Include(e => e.Tables)
            .ThenInclude(e => e.Tasks)
            .ThenInclude(e => e.Assignee)
            .FirstOrDefaultAsync(e => e.ID == id);

        if (found is null)
        {
            return new KanBoard();
        }

        return found;
    }

    public async Task UpdateAsync(KanBoard board)
    {
        var tracked = (await _dbContext.Boards.FindAsync(board.ID))!;

        tracked.FinishDate = board.FinishDate;
        tracked.CreationDate = board.CreationDate;
        tracked.StartDate = board.StartDate;
        tracked.StatusID = board.StatusID;
        tracked.BackgroundImageUrl = board.BackgroundImageUrl;
        tracked.Name = board.Name;

        _dbContext.Boards.Update(tracked);
    }
}
