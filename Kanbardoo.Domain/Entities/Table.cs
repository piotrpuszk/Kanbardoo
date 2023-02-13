using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;

public class Table : Entity
{
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public ICollection<KanTask> Tasks { get; set; } = new List<KanTask>();
}
