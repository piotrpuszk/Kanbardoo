using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class NewKanTaskDTO
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    [Required]
    public KanTaskStatusDTO Status { get; set; } = new KanTaskStatusDTO();
    public KanUserDTO? Assignee { get; set; } = new KanUserDTO();
    [Required]
    public int TableID { get; set; }
}