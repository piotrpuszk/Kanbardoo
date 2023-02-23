using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface IUserBoardRolesRepository
{
    Task AddAsync(KanUserBoardRole userBoardRole);
    Task DeleteAsync(KanUserBoardRole userBoardRole);
    Task DeleteAsync(int userID, int boardID);
    Task<KanUserBoardRole> GetAsync(KanUserBoardRole userBoardRole);
    Task<IEnumerable<KanUserBoardRole>> GetAsync(int userID, int boardID);
}
