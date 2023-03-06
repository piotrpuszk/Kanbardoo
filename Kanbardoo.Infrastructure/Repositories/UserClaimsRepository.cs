using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class UserClaimsRepository : IUserClaimsRepository
{
    private readonly DBContext _dbContext;

    public UserClaimsRepository(DBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task AddAsync(KanUserClaim userClaim)
    {
        userClaim.Claim = null!;
        userClaim.User = null!;
        userClaim.GeneratePrimaryKey();
        await _dbContext.UsersClaims.AddAsync(userClaim);
    }

    public async Task DeleteAsync(KanUserClaim userClaim)
    {
        await _dbContext.UsersClaims.Where(e => e.UserID == userClaim.UserID && e.ClaimID == userClaim.ClaimID).ExecuteDeleteAsync();
    }

    public async Task<KanUserClaim> GetAsync(KanUserClaim userClaim)
    {
        var result = await _dbContext.UsersClaims.FirstOrDefaultAsync(e => e.UserID == userClaim.UserID && e.ClaimID == userClaim.ClaimID);

        if (result == null)
        {
            return new KanUserClaim();
        }

        return result;
    }
}
