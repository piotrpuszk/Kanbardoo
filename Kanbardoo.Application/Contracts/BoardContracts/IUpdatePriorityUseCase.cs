using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.BoardContracts;

public interface IUpdatePriorityUseCase
{
    Task<Result> HandleAsync(KanBoard board);
}