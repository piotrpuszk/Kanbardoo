using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class NewKanBoardDTO
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
}
