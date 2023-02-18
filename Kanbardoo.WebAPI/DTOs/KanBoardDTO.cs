using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class KanBoardDTO
{
    [Required]
    public int ID { get; set; }
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public KanUserDTO Owner { get; set; } = new KanUserDTO();
    [Required]
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    [Required]
    public KanBoardStatusDTO Status { get; set; } = new KanBoardStatusDTO();
    [MaxLength(1024)]
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public ICollection<KanTableDTO> Tables { get; set; } = new List<KanTableDTO>();
}