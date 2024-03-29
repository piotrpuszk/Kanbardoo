﻿using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Threading.Tasks;

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
            .ThenInclude(e => e.Tasks.OrderBy(e => e.Priority))
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
        foreach (var table in board.Tables)
        {
            table.Board = null!;
            foreach (var task in table.Tasks)
            {
                task.Status = null!;
                task.Assignee = null!;
            }
        }

        _dbContext.ChangeTracker.Clear();
        var tracked = (await _dbContext.Boards.FindAsync(board.ID))!;

        tracked.FinishDate = board.FinishDate;
        tracked.CreationDate = board.CreationDate;
        tracked.StartDate = board.StartDate;
        tracked.StatusID = board.StatusID;
        tracked.BackgroundImageUrl = board.BackgroundImageUrl;
        tracked.Name = board.Name;
        tracked.Tables = board.Tables;
        foreach (var table in tracked.Tables)
        {
            _dbContext.Entry(table).State = EntityState.Modified;
        }
        foreach (var task in tracked.Tables.SelectMany(e => e.Tasks))
        {
            _dbContext.Entry(task).State = EntityState.Modified;
        }

        _dbContext.Boards.Update(tracked);
    }

    public async Task UpdatePriorityAsync(KanBoard board)
    {
        var boardDB = await GetAsync(board.ID);
        foreach (var table in board.Tables)
        {
            var tableDB = boardDB.Tables.First(e => e.ID == table.ID);
            tableDB.Priority = table.Priority;
            foreach (var task in table.Tasks)
            {
                var taskDB = tableDB.Tasks.FirstOrDefault(e => e.ID == task.ID);
                if (taskDB is null)
                {
                    var trackedTask = boardDB.Tables.SelectMany(e => e.Tasks).Where(e => e.ID == task.ID).First();
                    boardDB.Tables.First(e => e.ID == trackedTask.TableID).Tasks.Remove(trackedTask);
                    tableDB.Tasks.Add(trackedTask);
                }
                else
                {
                    taskDB.Priority = task.Priority;
                }
            }
        }
    }

    public async Task<IEnumerable<KanUser>> GetBoardMembers(int boardID)
    {
        var users = await _dbContext.UserBoardsRoles
            .Where(e => e.BoardID == boardID && e.RoleID == KanRoleID.Member)
            .Select(e => e.User)
            .ToListAsync();

        return users;
    }
}
