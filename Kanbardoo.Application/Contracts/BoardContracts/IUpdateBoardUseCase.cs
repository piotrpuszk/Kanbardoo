using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IUpdateBoardUseCase
{
    Task HandleAsync(Board board);
}
