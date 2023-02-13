using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Application.BoardUseCases;
public class DeleteBoardUseCase : IDeleteBoardUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoardUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(int id)
    {
        await _unitOfWork.BoardRepository.DeleteAsync(id);

        await _unitOfWork.SaveChangesAsync();
    }
}
