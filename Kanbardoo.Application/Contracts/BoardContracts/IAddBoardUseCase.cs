using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IAddBoardUseCase
{
    Task HandleAsync(NewBoard newBoard);
}
