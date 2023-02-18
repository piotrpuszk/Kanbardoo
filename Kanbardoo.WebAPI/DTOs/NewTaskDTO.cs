﻿using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.WebAPI.DTOs;

public sealed class NewTaskDTO
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    [Required]
    public TaskStatusDTO Status { get; set; } = new TaskStatusDTO();
    public UserDTO Assignee { get; set; } = new UserDTO();
    [Required]
    public int TableID { get; set; }
}