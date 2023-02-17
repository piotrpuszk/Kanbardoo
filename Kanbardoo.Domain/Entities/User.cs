using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;

public class User : Entity
{
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt { get; set; } = new byte[0];
}
