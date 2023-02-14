using Kanbardoo.Application.Results;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IDeleteBoardUseCase
{
    Task<Result> HandleAsync(int id);
}
