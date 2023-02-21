using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface IUserBoardsRepository
{
    Task<KanUserBoard> GetAsync(int userID, int boardID);
    Task AddAsync(KanUserBoard userBoard);
    Task DeleteAsync(KanUserBoard userBoard);
}