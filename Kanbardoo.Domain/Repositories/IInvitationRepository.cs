using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface IInvitationRepository
{
    Task AddAsync(Invitation invitation);
    Task<IEnumerable<Invitation>> GetAsync(int userID);
}