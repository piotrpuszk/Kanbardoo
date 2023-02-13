using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IUpdateTableUseCase
{
    Task HandleAsync(Table table);
}
