using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.TableContracts;
public interface IAddTableUseCase
{
    Task<Result> HandleAsync(NewTable newTable);
}
