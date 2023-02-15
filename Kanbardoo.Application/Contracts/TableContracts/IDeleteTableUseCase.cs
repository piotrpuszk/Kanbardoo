using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IDeleteTableUseCase
{
    Task<Result> HandleAsync(int id);
}
