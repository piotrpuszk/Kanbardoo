using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;
public interface IUserRepository
{
    Task<User> GetAsync(int id);
}
