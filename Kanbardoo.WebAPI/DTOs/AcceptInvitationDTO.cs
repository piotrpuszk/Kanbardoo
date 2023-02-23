using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class AcceptInvitationDTO
{
    [Required]
    public int ID { get; set; }
}
