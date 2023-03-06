using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

public class TableRepository : ITableRepository
{
    private readonly DBContext _dbContext;

    public TableRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(KanTable table)
    {
        table.Board = null!;
        table.GeneratePrimaryKey();
        await _dbContext.Tables.AddAsync(table);
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.Tables
            .Where(e => e.ID == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<KanTable>> GetAsync()
    {
        return await _dbContext.Tables
            .ToListAsync();
    }

    public async Task<KanTable> GetAsync(int id)
    {
        var found = await _dbContext.Tables
            .Include(e => e.Tasks.OrderBy(task => task.Priority))
            .ThenInclude(e => e.Status)
            .Include(e => e.Tasks)
            .ThenInclude(e => e.Assignee)
            .FirstOrDefaultAsync(e => e.ID == id);

        if (found is null)
        {
            return new KanTable();
        }

        return found;
    }

    public async Task UpdateAsync(KanTable table)
    {
        foreach (var task in table.Tasks)
        {
            task.Assignee = null!;
            task.Status = null!;
            task.Table = null!;
        }

        _dbContext.ChangeTracker.Clear();
        var tracked = (await _dbContext.Tables.FindAsync(table.ID))!;

        tracked.CreationDate = table.CreationDate;
        tracked.Priority = table.Priority;
        tracked.Name = table.Name;
        tracked.Tasks = table.Tasks;

        foreach (var task in tracked.Tasks)
        {
            _dbContext.Entry(task).State = EntityState.Modified;
        }

        _dbContext.Tables.Update(tracked);
    }
}