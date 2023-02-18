using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;

public class KanUser : Entity
{
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt { get; set; } = new byte[0];
    public ICollection<KanUserRole> Roles { get; set; } = new List<KanUserRole>();
    public ICollection<KanUserClaim> Claims { get; set; } = new List<KanUserClaim>();
}
