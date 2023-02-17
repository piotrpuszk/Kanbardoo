using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TaskContracts;

public interface IUpdateTaskUseCase
{
    Task<Result> HandleAsync(KanTask task);
}
