using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.TableContracts;
public interface IAddTableUseCase
{
    Task HandleAsync(NewTable newTable);
}
