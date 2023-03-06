using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;

namespace Kanbardoo.Domain.Repositories;
public interface IBoardRepository
{
    Task<IEnumerable<KanBoard>> GetAsync();
    Task<IEnumerable<KanBoard>> GetAsync(KanBoardFilters boardFilters);
    Task<KanBoard> GetAsync(int id);
    Task UpdateAsync(KanBoard board);
    Task UpdatePriorityAsync(KanBoard board);
    Task AddAsync(KanBoard board);
    Task DeleteAsync(int id);
    Task<IEnumerable<KanUser>> GetBoardMembers(int boardID);
}
