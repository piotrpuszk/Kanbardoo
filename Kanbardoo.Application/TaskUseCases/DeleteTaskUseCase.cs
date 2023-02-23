using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TaskUseCases;

public class DeleteTaskUseCase : IDeleteTaskUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly KanTaskIdToDeleteValidator _kanTaskIdToDeleteValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;
    private readonly int _userID;

    public DeleteTaskUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           KanTaskIdToDeleteValidator kanTaskIdToDeleteValidator,
                           IBoardMembershipPolicy boardMembershipPolicy,
                           IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _kanTaskIdToDeleteValidator = kanTaskIdToDeleteValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result> HandleAsync(int id)
    {
        var validationResult = await _kanTaskIdToDeleteValidator.ValidateAsync(id);
        if (!validationResult.IsValid)
        {
            _logger.Error($"A task with the given id ({id}) does not exist");
            return Result.ErrorResult(ErrorMessage.TaskWithIDNotExist);
        }

        var boardID = await _unitOfWork.ResourceIdConverterRepository.FromTaskIDToBoardID(id);
        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(boardID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        try
        {
            await _unitOfWork.TaskRepository.DeleteAsync(id);
            await _unitOfWork.UserTasksRepository.DeleteAsync(new KanUserTask { UserID = _userID, TaskID = id});
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
