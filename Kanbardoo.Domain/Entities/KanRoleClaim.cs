using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanRoleClaim : Entity
{
    [Required]
    public int RoleID { get; set; }

    [ForeignKey(nameof(RoleID))]
    public KanRole Role { get; set; } = new KanRole();

    [Required]
    public int ClaimID { get; set; }

    [ForeignKey(nameof(ClaimID))]
    public KanClaim Claim { get; set; } = new KanClaim();
}