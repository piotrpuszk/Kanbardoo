using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Contracts.TaskContracts;

public interface IDeleteTaskUseCase
{
    Task<Result> HandleAsync(int id);
}
