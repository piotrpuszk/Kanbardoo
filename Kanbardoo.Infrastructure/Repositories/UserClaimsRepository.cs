using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class UserClaimsRepository : IUserClaimsRepository
{
    private readonly DBContext _dBContext;

    public UserClaimsRepository(DBContext dBContext)
    {
        _dBContext = dBContext;
    }

    public async Task AddAsync(KanUserClaim userClaim)
    {
        userClaim.Claim = null!;
        userClaim.User = null!;
        await _dBContext.UsersClaims.AddAsync(userClaim);
    }
}

public class ClaimRepository : IClaimRepository
{
    private readonly DBContext _dBContext;

    public ClaimRepository(DBContext dBContext)
    {
        _dBContext = dBContext;
    }

    public async Task<KanClaim> GetAsync(int id)
    {
        var claim = await _dBContext.Claims.FirstOrDefaultAsync(e => e.ID == id);

        if (claim is null)
        {
            return new KanClaim();
        }

        return claim;
    }
}
