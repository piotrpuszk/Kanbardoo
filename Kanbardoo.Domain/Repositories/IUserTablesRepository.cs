using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface IUserTablesRepository
{
    Task<KanUserTable> GetAsync(int userID, int tableID);
    Task AddAsync(KanUserTable userTable);
    Task DeleteAsync(KanUserTable userTable);
}
