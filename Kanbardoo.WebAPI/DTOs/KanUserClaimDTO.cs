using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class KanUserClaimDTO
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string ClaimName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Value { get; set; } = string.Empty;
}
