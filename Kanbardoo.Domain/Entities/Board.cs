using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class Board : Entity
{
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public int OwnerID { get; set; }
    [ForeignKey(nameof(OwnerID))]
    public User? Owner { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public int StatusID { get; set; }
    [ForeignKey(nameof(StatusID))]
    public BoardStatus? Status { get; set; }
    [MaxLength(1024)]
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
}
