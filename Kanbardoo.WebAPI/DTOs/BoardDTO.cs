using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class BoardDTO
{
    [Required]
    public int ID { get; set; }
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public UserDTO Owner { get; set; } = new UserDTO();
    [Required]
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    [Required]
    public BoardStatusDTO Status { get; set; } = new BoardStatusDTO();
    [MaxLength(1024)]
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public ICollection<TableDTO> Tables { get; set; } = new List<TableDTO>();
}