namespace Kanbardoo.Application.Contracts.BoardContracts;
public interface IDeleteBoardUseCase
{
    Task HandleAsync(int id);
}
