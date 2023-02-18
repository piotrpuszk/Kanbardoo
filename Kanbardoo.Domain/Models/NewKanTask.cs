namespace Kanbardoo.Domain.Models;

public sealed class NewKanTask
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int StatusID { get; set; }
    public int AssigneeID { get; set; }
    public int TableID { get; set; }
}