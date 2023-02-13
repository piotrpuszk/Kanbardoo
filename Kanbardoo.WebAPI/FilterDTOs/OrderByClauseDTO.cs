using Kanbardoo.Domain.Filters;

namespace Kanbardoo.WebAPI.FilterDTOs;

[Serializable]
public class OrderByClauseDTO
{
    public string ColumnName { get; set; } = string.Empty;
    public OrderByOrder Order { get; set; } = OrderByOrder.Asc;
}
