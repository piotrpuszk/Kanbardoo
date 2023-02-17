using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TaskContracts;

public interface IGetTaskUseCase
{
    Task<Result<KanTask>> HandleAsync(int id); 
}