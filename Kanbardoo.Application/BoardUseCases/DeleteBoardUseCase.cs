using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using ILogger = Serilog.ILogger;  

namespace Kanbardoo.Application.BoardUseCases;
public class DeleteBoardUseCase : IDeleteBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BoardIdToDeleteValidator _boardIdToDeleteValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;
    private readonly int _userID;

    public DeleteBoardUseCase(IUnitOfWork unitOfWork,
                              ILogger logger,
                              BoardIdToDeleteValidator boardIdToDeleteValidator,
                              IBoardMembershipPolicy boardMembershipPolicy,
                              IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _boardIdToDeleteValidator = boardIdToDeleteValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result> HandleAsync(int id)
    {
        var validationResult = await _boardIdToDeleteValidator.ValidateAsync(id);
        if (!validationResult.IsValid)
        {
            _logger.Error($"A board with given ID does not exist: {id}");
            return Result.ErrorResult(ErrorMessage.BoardWithIDNotExist);
        }

        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(id);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        try
        {
            return await DeleteFromDatabase(id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error during delete from database: {id} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    private async Task<Result> DeleteFromDatabase(int id)
    {
        await _unitOfWork.BoardRepository.DeleteAsync(id);
        await _unitOfWork.UserBoardRolesRepository.DeleteAsync(_userID, id);
        await _unitOfWork.SaveChangesAsync();

        return Result.SuccessResult();
    }
}
