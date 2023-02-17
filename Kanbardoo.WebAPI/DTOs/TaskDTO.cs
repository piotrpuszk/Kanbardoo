namespace Kanbardoo.WebAPI.DTOs;

public sealed class KanTaskDTO
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public TaskStatusDTO Status { get; set; } = new TaskStatusDTO();
    public UserDTO Assignee { get; set; } = new UserDTO();
}
