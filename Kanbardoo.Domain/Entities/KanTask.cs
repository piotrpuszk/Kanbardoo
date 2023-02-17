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
    public TaskStatus Status { get; set; } = new TaskStatus();
    public int AssigneeID { get; set; }

    [ForeignKey(nameof(AssigneeID))]
    public User Assignee { get; set; } = new User();
    public int TableID { get; set; }
    [ForeignKey(nameof(TableID))]
    public Table Table { get; set; } = new Table();
}