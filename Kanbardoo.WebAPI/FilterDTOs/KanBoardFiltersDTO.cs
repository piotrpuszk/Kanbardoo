using Kanbardoo.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.FilterDTOs;
public class KanBoardFiltersDTO
{
    [MaxLength(256)]
    public string BoardName { get; set; } = string.Empty;
    [MaxLength(10)]
    public IEnumerable<OrderByClauseDTO> OrderByClauses { get; set; } = new List<OrderByClauseDTO>();
    public int RoleID { get; set; } = KanRoleID.Owner;
}
