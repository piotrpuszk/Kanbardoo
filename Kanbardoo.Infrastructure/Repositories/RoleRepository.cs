using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DBContext _dBContext;

    public RoleRepository(DBContext dBContext)
    {
        _dBContext = dBContext;
    }

    public async Task<KanRole> GetAsync(int id)
    {
        var role = await _dBContext.Roles.FirstOrDefaultAsync(e => e.ID == id);

        if (role is null)
        {
            return new KanRole();
        }

        return role;
    }

    public async Task<KanRole> GetAsync(string name)
    {
        var role = await _dBContext.Roles.FirstOrDefaultAsync(e => e.Name == name);

        if (role is null)
        {
            return new KanRole();
        }

        return role;
    }
}
