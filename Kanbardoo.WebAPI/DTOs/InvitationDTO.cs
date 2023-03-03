using Kanbardoo.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class InvitationDTO
{
    [Required]
    public int ID { get; set; }

    [Required]
    public KanUserDTO User { get; set; } = new KanUserDTO();

    [Required]
    public KanBoardDTO Board { get; set; } = new KanBoardDTO();
    [Required]
    public KanUserDTO Sender { get; set; } = new KanUserDTO();
}
