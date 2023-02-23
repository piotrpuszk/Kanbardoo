using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Models;

public class UserRoleRevokeModel
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string RoleName { get; set; } = string.Empty;
}
