using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IUpdateTableUseCase
{
    Task<Result> HandleAsync(KanTable table);
}
