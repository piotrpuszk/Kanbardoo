using System.ComponentModel.DataAnnotations;

namespace Kanbardoo.Domain.Entities;

public class KanUser : Entity
{
    [MaxLength(256)]
    public string UserName { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt { get; set; } = new byte[0];
    public ICollection<KanUserRole> Roles { get; set; } = new List<KanUserRole>();
    public ICollection<KanUserClaim> Claims { get; set; } = new List<KanUserClaim>();

    public IEnumerable<(string Name, List<string> Value)> GetClaimList()
    {
        var claimDictionary = new Dictionary<string, List<string>>();

        foreach (var claim in Claims)
        {
            if (!claimDictionary.ContainsKey(claim.Claim.Name))
            {
                claimDictionary[claim.Claim.Name] = new List<string>();
            }

            claimDictionary[claim.Claim.Name].Add(claim.Value);
        }

        return claimDictionary.ToList().Select(e => (e.Key, e.Value));
    }
}
