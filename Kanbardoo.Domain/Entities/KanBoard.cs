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
    public ICollection<KanTable> Tables { get; set; } = new List<KanTable>();
    public ICollection<KanUserBoardRole> UserBoardRoles { get; set; } = new List<KanUserBoardRole>();

    public static KanBoard CreateFromNewBoard(NewKanBoard newBoard, int ownerID)
    {
        KanBoard board = new()
        {
            Name = newBoard.Name,
            CreationDate = DateTime.UtcNow,
            StatusID = KanBoardStatusId.Active,
            OwnerID = ownerID,
        };
        board.GeneratePrimaryKey();
        return board;
    }
}
