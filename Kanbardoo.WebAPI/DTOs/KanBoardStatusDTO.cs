﻿using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class KanBoardStatusDTO
{
    [Required]
    public int ID { get; set; }
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
}
