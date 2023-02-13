namespace Kanbardoo.WebAPI.DTOs;

public sealed class BoardDTO
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public UserDTO Owner { get; set; } = new UserDTO();
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public BoardStatusDTO Status { get; set; } = new BoardStatusDTO();
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public ICollection<TableDTO> Tables { get; set; } = new List<TableDTO>();
}