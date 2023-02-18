using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class KanUserDTO
{
    [Required]
    public int ID { get; set; }

    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public DateTime CreationDate { get; set; }
}