using Kanbardoo.Domain.Filters;
using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.FilterDTOs;

[Serializable]
public class OrderByClauseDTO
{
    [MaxLength(256)]
    public string ColumnName { get; set; } = string.Empty;
    public OrderByOrder Order { get; set; } = OrderByOrder.Asc;
}
