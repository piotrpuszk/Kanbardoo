using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanUserTable : Entity
{
    [Required]
    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))] 
    public KanUser User { get; set; } = new KanUser(); 

    [Required]
    public int TableID { get; set; }

    [ForeignKey(nameof(TableID))] 
    public KanTable Table { get; set; } = new KanTable();
}
