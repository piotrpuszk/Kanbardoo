using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public sealed class AddBoardUseCase : IAddBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddBoardUseCase(IUnitOfWork unitOfWork,
                           ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(NewBoard newBoard)
    {
        Board board = new()
        {
            Name= newBoard.Name,
            CreationDate = DateTime.UtcNow,
            StatusID = BoardStatusId.Active,
            OwnerID = 46920,
        };

        await _unitOfWork.BoardRepository.AddAsync(board);

        await _unitOfWork.SaveChangesAsync();
    }
}
