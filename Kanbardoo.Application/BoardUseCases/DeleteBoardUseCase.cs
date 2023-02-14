using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;  

namespace Kanbardoo.Application.BoardUseCases;
public class DeleteBoardUseCase : IDeleteBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoardUseCase(IUnitOfWork unitOfWork,
                              ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(int id)
    {
        var board = await _unitOfWork.BoardRepository.GetAsync(id);

        if (board.ID == default)
        {
            _logger.Error($"A board with given ID does not exist: {id}");
            return Result.ErrorResult("A board with given ID does not exist");
        }

        try
        {
            return await DeleteFromDatabase(id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error during delete from database: {id}");
            return Result.ErrorResult("Internal server error");
        }
    }

    private async Task<Result> DeleteFromDatabase(int id)
    {
        await _unitOfWork.BoardRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return Result.SuccessResult();
    }
}
