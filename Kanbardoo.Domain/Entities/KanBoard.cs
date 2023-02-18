using Kanbardoo.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanBoard : Entity
{
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public int OwnerID { get; set; }
    [ForeignKey(nameof(OwnerID))]
    public KanUser? Owner { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public int StatusID { get; set; }
    [ForeignKey(nameof(StatusID))]
    public KanBoardStatus? Status { get; set; }
    [MaxLength(1024)]
    public string BackgroundImageUrl { get; set; } = string.Empty;
    public virtual ICollection<KanTable> Tables { get; set; } = new List<KanTable>();

    public static KanBoard CreateFromNewBoard(NewKanBoard newBoard)
    {
        return new()
        {
            Name = newBoard.Name,
            CreationDate = DateTime.UtcNow,
            StatusID = KanBoardStatusId.Active,
            OwnerID = 46920,
        };
    }
}
