using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class GetTableDTO
{
    [Required]
    public int ID { get; set; }
    [Required]
    public int BoardID { get; set; }
}