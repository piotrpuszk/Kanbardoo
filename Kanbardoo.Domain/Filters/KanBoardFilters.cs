using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Domain.Filters;
public class KanBoardFilters
{
    public string BoardName { get; set; } = string.Empty;
    public IEnumerable<OrderByClause<KanBoard>> OrderByClauses { get; set; } = Enumerable.Empty<OrderByClause<KanBoard>>();
    public int OwnerID { get; set; }
    public int RoleID { get; set; } = KanRoleID.Owner;
}
