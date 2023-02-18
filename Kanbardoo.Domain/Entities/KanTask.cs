using Kanbardoo.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanTask : Entity
{
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int StatusID { get; set; }

    [ForeignKey(nameof(StatusID))]
    public KanTaskStatus Status { get; set; } = new KanTaskStatus();
    public int AssigneeID { get; set; }

    [ForeignKey(nameof(AssigneeID))]
    public KanUser Assignee { get; set; } = new KanUser();
    public int TableID { get; set; }
    [ForeignKey(nameof(TableID))]
    public KanTable Table { get; set; } = new KanTable();
}