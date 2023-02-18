using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;
public interface IUserClaimsRepository
{
    Task AddAsync(KanUserClaim userClaim);
}
