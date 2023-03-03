using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.BoardContracts;

public interface IGetBoardMembersUseCase
{
    Task<Result<IEnumerable<KanBoardUser>>> HandleAsync(int boardID);
}