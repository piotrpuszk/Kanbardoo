using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Models;

public class UserBoardRoleGrantModel
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string RoleName { get; set; } = string.Empty;

    [Required]
    public int BoardID { get; set; }
}
