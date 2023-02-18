using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;

public class KanTaskStatus : Entity
{
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
}