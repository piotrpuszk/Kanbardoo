using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;

namespace Kanbardoo.Domain.Repositories;
public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetAsync();
    Task<IEnumerable<Board>> GetAsync(BoardFilters boardFilters);
    Task<Board> GetAsync(int id);
    Task UpdateAsync(Board board);
    Task AddAsync(Board board);
    Task DeleteAsync(int id);
}

public interface ITableRepository
{
    Task<IEnumerable<Table>> GetAsync();
    Task<Table> GetAsync(int id);
    Task UpdateAsync(Table board);
    Task AddAsync(Table board);
    Task DeleteAsync(int id);
}
