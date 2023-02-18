using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;
public class KanRole : Entity
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public ICollection<KanClaim> Claims { get; set; } = new List<KanClaim>();
    public ICollection<KanUserRole> Users { get; set; } = new List<KanUserRole>();
}
