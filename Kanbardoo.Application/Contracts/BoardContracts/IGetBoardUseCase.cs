using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IGetBoardUseCase
{
    Task<Result<IEnumerable<KanBoard>>> HandleAsync();
    Task<Result<IEnumerable<KanBoard>>> HandleAsync(KanBoardFilters boardFilters);
    Task<Result<KanBoard>> HandleAsync(int id);
}
