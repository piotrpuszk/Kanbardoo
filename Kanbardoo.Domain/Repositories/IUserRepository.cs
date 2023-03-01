using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;
public interface IUserRepository
{
    Task<KanUser> GetAsync(int id);
    Task<KanUser> GetAsync(string name);
    Task AddAsync(KanUser user);
    Task<IEnumerable<KanUser>> GetUsersAsync(string query);
}
