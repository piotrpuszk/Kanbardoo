using Kanbardoo.Domain.Filters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kanbardoo.WebAPI.FilterDTOs;
public class BoardFiltersDTO
{
    [MaxLength(256)]
    public string BoardName { get; set; } = string.Empty;
    [MaxLength(10)]
    public IEnumerable<OrderByClauseDTO> OrderByClauses { get; set; } = new List<OrderByClauseDTO>();
}
