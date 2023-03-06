using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;

public class DeleteTableUseCase : IDeleteTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TableIDToDelete _tableIDToDelete;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;
    private readonly int _userID;

    public DeleteTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           TableIDToDelete tableIDToDelete,
                           IBoardMembershipPolicy boardMembershipPolicy,
                           IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _tableIDToDelete = tableIDToDelete;
        _boardMembershipPolicy = boardMembershipPolicy;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result> HandleAsync(int id)
    {
        var validationResult = await _tableIDToDelete.ValidateAsync(id);
        if (!validationResult.IsValid)
        {
            _logger.Error($"The table id is invalid {id}");
            return Result.ErrorResult(ErrorMessage.TableIDInvalid);
        }

        var boardID = await _unitOfWork.ResourceIdConverterRepository.FromTableIDToBoardID(id);
        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(boardID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        try
        {
            await _unitOfWork.TableRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error: {id} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
        
    }
}
