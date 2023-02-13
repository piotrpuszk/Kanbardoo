using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Application.BoardUseCases;
public class UpdateBoardUseCase : IUpdateBoardUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(Board board)
    {
        await _unitOfWork.BoardRepository.UpdateAsync(board);

        await _unitOfWork.SaveChangesAsync();
    }
}
