using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class KanTableDTO
{
    [Required]
    public int ID { get; set; }
    [Required]
    public int BoardID { get; set; }
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public DateTime CreationDate { get; set; }
    public ICollection<KanTaskDTO> Tasks { get; set; } = new List<KanTaskDTO>();
}
