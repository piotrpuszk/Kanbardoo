using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kanbardoo.Infrastructure.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly DBContext _dbContext;

    public InvitationRepository(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Invitation invitation)
    {
        invitation.Board = null!;
        invitation.User= null!;
        await _dbContext.Invitations.AddAsync(invitation);
    }

    public async Task DeleteAsync(Invitation invitation)
    {
        await _dbContext.Invitations
            .Where(e => e.UserID == invitation.UserID && e.BoardID == invitation.BoardID)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Invitation>> GetAsync(int userID)
    {
        return await _dbContext.Invitations
            .Where(e => e.UserID == userID)
            .Include(e => e.Board)
            .ThenInclude(e => e.Owner)
            .Include(e => e.Board)
            .ThenInclude(e => e.Status)
            .Include(e => e.User)
            .ToListAsync();
    }
}