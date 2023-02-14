using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IGetBoardUseCase
{
    Task<Result<IEnumerable<Board>>> HandleAsync();
    Task<Result<IEnumerable<Board>>> HandleAsync(BoardFilters boardFilters);
    Task<Result<Board>> HandleAsync(int id);
}
