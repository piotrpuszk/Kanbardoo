namespace Kanbardoo.WebAPI.DTOs;

public sealed class TableDTO
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public ICollection<KanTaskDTO> Tasks { get; set; } = new List<KanTaskDTO>();
}
