﻿using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public class NewKanTableDTO
{
    [Required]
    public int BoardID { get; set; }
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public int Priority { get; set; }
}
