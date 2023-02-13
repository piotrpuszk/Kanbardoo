using Kanbardoo.Domain.Filters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kanbardoo.WebAPI.FilterDTOs;
public class BoardFiltersDTO
{
    public string BoardName { get; set; } = string.Empty;

    public IEnumerable<OrderByClauseDTO> OrderByClauses { get; set; } = new List<OrderByClauseDTO>();
}
