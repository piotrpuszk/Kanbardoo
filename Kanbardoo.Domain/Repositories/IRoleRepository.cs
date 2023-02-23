using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Domain.Repositories;

public interface IRoleRepository
{
    Task<KanRole> GetAsync(int id);
    Task<KanRole> GetAsync(string name);
}

