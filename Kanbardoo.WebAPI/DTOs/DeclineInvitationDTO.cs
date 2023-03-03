using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class DeclineInvitationDTO
{
    [Required]
    public int InvitationID { get; set; }
}
