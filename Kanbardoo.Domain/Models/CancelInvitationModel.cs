namespace Kanbardoo.Domain.Models;

public class CancelInvitationModel
{
    public string UserName { get; set; } = string.Empty;
    public int BoardID { get; set; }
}