using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Filters;
public class BoardFilters
{
    public string BoardName { get; set; } = string.Empty;
    public IEnumerable<OrderByClause<Board>> OrderByClauses { get; set; } = Enumerable.Empty<OrderByClause<Board>>();

    public bool IsValid()
    {
        foreach (var item in OrderByClauses)
        {
            if (!Entity.ColumnExists<Board>(item.ColumnName))
            {
                return false;
            }
        }

        return true;
    }
}
