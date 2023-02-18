using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanUserRole : Entity
{
    [Required]
    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public KanUser User { get; set; } = new KanUser();

    [Required]
    public int RoleID { get; set; }

    [ForeignKey(nameof(RoleID))]
    public KanRole Role { get; set; } = new KanRole();
}