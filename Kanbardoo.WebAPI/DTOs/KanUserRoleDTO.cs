using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class KanUserRoleDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int UserID { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int RoleID { get; set; }
}
