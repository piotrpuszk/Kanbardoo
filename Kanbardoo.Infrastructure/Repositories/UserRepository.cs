using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly DBContext _dbContext;

    public UserRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<KanUser> GetAsync(int id)
    {
        var user = await _dbContext.Users
            .Include(e => e.Claims)
            .ThenInclude(e => e.Claim)
            .FirstOrDefaultAsync(e => e.ID == id);

        if (user is null)
        {
            return new KanUser();
        }

        return user;
    }

    public async Task<KanUser> GetAsync(string name)
    {
        var user = await _dbContext.Users
            .Include(e => e.Claims)
            .ThenInclude(e => e.Claim)
            .FirstOrDefaultAsync(e => e.UserName == name);

        if (user is null)
        {
            return new KanUser();
        }

        return user;
    }

    public async Task AddAsync(KanUser user)
    {
        user.Roles = null!;
        user.Claims = null!;
        await _dbContext.Users.AddAsync(user);
    }
}
