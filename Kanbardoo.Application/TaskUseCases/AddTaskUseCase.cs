using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TaskContracts;
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

namespace Kanbardoo.Application.TaskUseCases;
public class AddTaskUseCase : IAddTaskUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NewTaskValidator _newTaskValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;
    private readonly int _userID;

    public AddTaskUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           NewTaskValidator newTaskValidator,
                           IBoardMembershipPolicy boardMembershipPolicy,
                           IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _newTaskValidator = newTaskValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result> HandleAsync(NewKanTask newTask)
    {
        var validationResult = await _newTaskValidator.ValidateAsync(newTask);
        if (!validationResult.IsValid)
        {
            if(newTask is not null) _logger.Error($"the new task is invalid \n\n {JsonConvert.SerializeObject(newTask)}");
            else _logger.Error($"the new task is null \n\n");
            return Result.ErrorResult(ErrorMessage.GivenTaskInvalid);
        }

        var boardID = await _unitOfWork.ResourceIdConverterRepository.FromTableIDToBoardID(newTask.TableID);
        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(boardID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        KanTask task = new()
        {
            TableID = newTask.TableID,
            Name = newTask.Name,
            Description = newTask.Description,
            DueDate = newTask.DueDate,
            StatusID = newTask.StatusID,
            AssigneeID = newTask.AssigneeID,
        };

        try
        {
            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.UserTasksRepository.AddAsync(new KanUserTask { UserID = _userID, TaskID = task.ID });
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error: \n\n {nameof(AddTaskUseCase)}.{nameof(HandleAsync)}({JsonConvert.SerializeObject(newTask)}) \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }
}
