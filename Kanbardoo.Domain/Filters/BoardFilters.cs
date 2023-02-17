using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Filters;
public class BoardFilters
{
    public string BoardName { get; set; } = string.Empty;
    public IEnumerable<OrderByClause<Board>> OrderByClauses { get; set; } = Enumerable.Empty<OrderByClause<Board>>();
}
