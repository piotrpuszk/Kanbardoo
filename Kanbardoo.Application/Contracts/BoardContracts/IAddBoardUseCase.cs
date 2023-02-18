using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IAddBoardUseCase
{
    Task<Result> HandleAsync(NewKanBoard newBoard);
}
