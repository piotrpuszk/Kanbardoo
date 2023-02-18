using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class SignUpDTO
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string Password { get; set; } = string.Empty;
}
