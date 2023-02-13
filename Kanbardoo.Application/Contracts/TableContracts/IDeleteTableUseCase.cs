namespace Kanbardoo.Application.Contracts.TableContracts;

public interface IDeleteTableUseCase
{
    Task HandleAsync(int id);
}
