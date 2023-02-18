using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Filters;
public class KanBoardFilters
{
    public string BoardName { get; set; } = string.Empty;
    public IEnumerable<OrderByClause<KanBoard>> OrderByClauses { get; set; } = Enumerable.Empty<OrderByClause<KanBoard>>();
}
