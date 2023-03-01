
using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TaskStatusContracts;
public interface IGetTaskStatusesUseCase
{
    Task<Result<IEnumerable<KanTaskStatus>>> HandleAsync();
}
