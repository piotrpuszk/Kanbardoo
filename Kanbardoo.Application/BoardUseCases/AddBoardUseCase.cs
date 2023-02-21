using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public sealed class AddBoardUseCase : IAddBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NewBoardValidator _newBoardValidator;
    private readonly int _userID;

    public AddBoardUseCase(IUnitOfWork unitOfWork,
                           ILogger logger,
                           NewBoardValidator newBoardValidator,
                           IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newBoardValidator = newBoardValidator;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result> HandleAsync(NewKanBoard newBoard)
    {
        var validationResult = _newBoardValidator.Validate(newBoard);
        if (!validationResult.IsValid)
        {
            _logger.Error($"{nameof(AddBoardUseCase)}.{nameof(HandleAsync)} => newBoard is invalid");
            return Result.ErrorResult(ErrorMessage.NewTableInvalid);
        }

        try
        {
            return await SaveBoardInDatabase(newBoard);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error during adding a new board: {JsonConvert.SerializeObject(newBoard)}" + $"\n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    private async Task<Result> SaveBoardInDatabase(NewKanBoard newBoard)
    {
        KanBoard board = KanBoard.CreateFromNewBoard(newBoard);

        await _unitOfWork.BoardRepository.AddAsync(board);
        var addedItemsCount = await _unitOfWork.SaveChangesAsync();

        await _unitOfWork.UserBoardsRepository.AddAsync(new KanUserBoard { UserID = _userID, BoardID = board.ID });
        await _unitOfWork.SaveChangesAsync();

        if (addedItemsCount < 0)
        {
            _logger.Error($"no board has been saved: {JsonConvert.SerializeObject(newBoard)} => {JsonConvert.SerializeObject(board)}");
            return Result.ErrorResult(ErrorMessage.NoBoardSaved);
        }

        return Result.SuccessResult();
    }
}
