using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

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

    public async Task<KanClaim> GetAsync(string name)
    {
        var claim = await _dBContext.Claims.FirstOrDefaultAsync(e => e.Name == name);

        if (claim is null)
        {
            return new KanClaim();
        }

        return claim;
    }
}
