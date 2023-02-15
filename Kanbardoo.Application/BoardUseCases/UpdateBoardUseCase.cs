using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Newtonsoft.Json;
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

    public async Task<Result> HandleAsync(Board board)
    {
        if (board is null)
        {
            _logger.Error($"{nameof(UpdateBoardUseCase)}.{nameof(HandleAsync)} board is null");
            return Result.ErrorResult("Board is null");
        }

        var found = await _unitOfWork.BoardRepository.GetAsync(board.ID);

        if (!found.Exists())
        {
            _logger.Error($"Board with the ID {board.ID} does not exist in the db");
            return Result.ErrorResult("The board does not exist");
        }

        try
        {
            await _unitOfWork.BoardRepository.UpdateAsync(board);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"{JsonConvert.SerializeObject(board)} \n\n {ex}");
            return Result.ErrorResult("Internal server error");
        }
        
    }
}
