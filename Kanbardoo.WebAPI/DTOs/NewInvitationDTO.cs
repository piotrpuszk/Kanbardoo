using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class NewInvitationDTO
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public int BoardID { get; set; }
}
