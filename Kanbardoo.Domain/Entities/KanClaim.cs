using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;

public class KanClaim : Entity
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public ICollection<KanUserClaim> Users { get; set; } = new List<KanUserClaim>();
    public ICollection<KanRoleClaim> Roles { get; set; } = new List<KanRoleClaim>();
}
