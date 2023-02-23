using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Domain.Repositories;

public interface IInvitationRepository
{
    Task AddAsync(Invitation invitation);
    Task<IEnumerable<Invitation>> GetUserInvitationsAsync(int userID);
    Task<Invitation> GetAsync(int id);
    Task DeleteAsync(Invitation invitation);
    Task DeleteAsync(int id);
}