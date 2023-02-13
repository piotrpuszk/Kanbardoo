using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IGetBoardUseCase
{
    Task<IEnumerable<Board>> HandleAsync();
    Task<IEnumerable<Board>> HandleAsync(BoardFilters boardFilters);
    Task<Board> HandleAsync(int id);
}
