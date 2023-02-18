using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.TaskContracts;
public interface IAddTaskUseCase
{
    Task<Result> HandleAsync(NewKanTask newTask);
}
