﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Entities;

public class KanTable : Entity
{
    public int BoardID { get; set; }
    [ForeignKey(nameof(BoardID))]
    public KanBoard Board { get; set; } = new KanBoard();
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public int Priority { get; set; }
    public ICollection<KanTask> Tasks { get; set; } = new List<KanTask>();
}
