using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public class UpdateBoardUseCase : IUpdateBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBoardUseCase(IUnitOfWork unitOfWork,
                              ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(Board board)
    {
        await _unitOfWork.BoardRepository.UpdateAsync(board);

        await _unitOfWork.SaveChangesAsync();
    }
}
