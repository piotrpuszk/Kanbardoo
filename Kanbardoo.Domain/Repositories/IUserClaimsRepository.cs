using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;
public interface IUserClaimsRepository
{
    Task AddAsync(KanUserClaim userClaim);
    Task DeleteAsync(KanUserClaim userClaim);
    Task<KanUserClaim> GetAsync(KanUserClaim userClaim);
}
