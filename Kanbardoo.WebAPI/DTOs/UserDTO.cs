namespace Kanbardoo.WebAPI.DTOs;

public sealed class UserDTO
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
}