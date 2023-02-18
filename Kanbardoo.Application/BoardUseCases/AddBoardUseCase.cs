using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public sealed class AddBoardUseCase : IAddBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NewBoardValidator _newBoardValidator;

    public AddBoardUseCase(IUnitOfWork unitOfWork,
                           ILogger logger,
                           NewBoardValidator newBoardValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newBoardValidator = newBoardValidator;
    }

    public async Task<Result> HandleAsync(NewBoard newBoard)
    {
        var validationResult = _newBoardValidator.Validate(newBoard);
        if (!validationResult.IsValid)
        {
            _logger.Error($"{nameof(AddBoardUseCase)}.{nameof(HandleAsync)} => newBoard is invalid");
            return Result.ErrorResult("A new board is invalid");
        }

        try
        {
            return await SaveBoardInDatabase(newBoard);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error during adding a new board: {JsonConvert.SerializeObject(newBoard)}" + $"\n\n {ex}");
            return Result.ErrorResult("Internal server error", HttpStatusCode.InternalServerError);
        }
    }

    private async Task<Result> SaveBoardInDatabase(NewBoard newBoard)
    {
        Board board = Board.CreateFromNewBoard(newBoard);

        await _unitOfWork.BoardRepository.AddAsync(board);

        var addedItemsCount = await _unitOfWork.SaveChangesAsync();

        if (addedItemsCount < 0)
        {
            _logger.Error($"no board has been saved: {JsonConvert.SerializeObject(newBoard)} => {JsonConvert.SerializeObject(board)}");
            return Result.ErrorResult($"no board has been saved");
        }

        return Result.SuccessResult();
    }
}
