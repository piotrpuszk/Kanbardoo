using Kanbardoo.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanbardoo.Domain.Models;
public class NewInvitation
{
    public string UserName { get; set; } = string.Empty;
    public int BoardID { get; set; }
}

public class AcceptInvitation
{
    public int ID { get; set; }
}