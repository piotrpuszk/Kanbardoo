using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class KanUserClaimDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int UserID { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ClaimID { get; set; }
    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;
}
