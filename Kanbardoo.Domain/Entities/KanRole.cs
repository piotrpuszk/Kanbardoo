using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;
public class KanRole : Entity
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public ICollection<KanRoleClaim> Claims { get; set; } = new List<KanRoleClaim>();
    public ICollection<KanUserBoardRole> Users { get; set; } = new List<KanUserBoardRole>();
}
