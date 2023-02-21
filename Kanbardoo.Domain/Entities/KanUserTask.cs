using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanUserTask : Entity
{
    [Required]
    public int UserID { get; set; }
    [ForeignKey(nameof(UserID))]
    public KanUser User { get; set; } = new KanUser();
    [Required]
    public int TaskID { get; set; }
    [ForeignKey(nameof(TaskID))]
    public KanTask Task { get; set; } = new KanTask();
}