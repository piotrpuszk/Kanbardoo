using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IUpdateBoardUseCase
{
    Task<Result> HandleAsync(KanBoard board);
}
