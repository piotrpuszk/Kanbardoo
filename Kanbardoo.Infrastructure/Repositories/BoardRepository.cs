﻿using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
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

    public async Task AddAsync(Board board)
    {
        await _dbContext.Boards.AddAsync(board);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.Boards.Where(e => e.ID == id).ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Board>> GetAsync()
    {
        return await _dbContext.Boards
            .Include(e => e.Owner)
            .Include(e => e.Status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Board>> GetAsync(BoardFilters boardFilters)
    {
        var query = _dbContext.Boards
            .Include(e => e.Owner)
            .Include(e => e.Status)
            .AsQueryable();

        var boardName = boardFilters.BoardName;
        if (!string.IsNullOrWhiteSpace(boardName))
        {
            query = query.Where(e => e.Name.ToLower().Contains(boardName.ToLower()));
        }

        query = query.OrderByClauses(boardFilters.OrderByClauses);

        return await query.ToListAsync();
    }

    public async Task<Board> GetAsync(int id)
    {
        var found = await _dbContext.Boards
            .Include(e => e.Owner)
            .Include(e => e.Status)
            .FirstOrDefaultAsync(e => e.ID == id);

        if (found is null)
        {
            return new Board();
        }

        return found;
    }

    public async Task UpdateAsync(Board board)
    {
        board.Status = null;
        board.Owner = null;
        _dbContext.Boards.Update(board);
    }
}