using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using ILogger = Serilog.ILogger;  

namespace Kanbardoo.Application.BoardUseCases;
public class DeleteBoardUseCase : IDeleteBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BoardIdToDeleteValidator _boardIdToDeleteValidator;

    public DeleteBoardUseCase(IUnitOfWork unitOfWork,
                              ILogger logger,
                              BoardIdToDeleteValidator boardIdToDeleteValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _boardIdToDeleteValidator = boardIdToDeleteValidator;
    }

    public async Task<Result> HandleAsync(int id)
    {
        var validationResult = await _boardIdToDeleteValidator.ValidateAsync(id);
        if (!validationResult.IsValid)
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
            _logger.Error($"Error during delete from database: {id} \n\n {ex}");
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
