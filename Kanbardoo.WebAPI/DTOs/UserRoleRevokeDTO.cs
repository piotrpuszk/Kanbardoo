﻿using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class UserRoleRevokeDTO
{
    [Required]
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string RoleName { get; set; } = string.Empty;
}