using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanUserClaim : Entity
{
    [Required]
    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public KanUser User { get; set; } = new KanUser();

    [Required]
    public int ClaimID { get; set; }

    [ForeignKey(nameof(ClaimID))]
    public KanClaim Claim { get; set; } = new KanClaim();

    [Required]
    [MaxLength(256)]
    public string Value { get; set; } = string.Empty;
}
