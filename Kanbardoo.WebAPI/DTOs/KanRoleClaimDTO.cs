using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class KanRoleClaimDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int RoleID { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ClaimID { get; set; }
}