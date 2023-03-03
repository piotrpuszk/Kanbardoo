using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class Invitation : Entity
{
    [Required]
    public int UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public KanUser User { get; set; } = new KanUser();
    public int BoardID { get; set; }
    [ForeignKey(nameof(BoardID))]
    public KanBoard Board { get; set; } = new KanBoard();
    public int SenderID { get; set; }
    [ForeignKey(nameof(SenderID))]
    public KanUser Sender { get; set; }
}