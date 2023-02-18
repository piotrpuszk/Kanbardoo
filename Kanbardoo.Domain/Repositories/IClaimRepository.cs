using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface IClaimRepository
{
    Task<KanClaim> GetAsync(int id);

}