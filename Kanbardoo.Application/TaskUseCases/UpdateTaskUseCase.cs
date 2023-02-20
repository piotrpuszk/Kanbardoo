using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TaskUseCases;

public class UpdateTaskUseCase : IUpdateTaskUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly KanTaskValidator _validator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;

    public UpdateTaskUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           KanTaskValidator validator,
                           IBoardMembershipPolicy boardMembershipPolicy)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _boardMembershipPolicy = boardMembershipPolicy;
    }

    public async Task<Result> HandleAsync(KanTask task)
    {
        var validationResult = await _validator.ValidateAsync(task);
        if (task is null || !validationResult.IsValid)
        {
            _logger.Error($"Invalid task to update: {JsonConvert.SerializeObject(task is not null ? task : "null")}");
            return Result.ErrorResult(ErrorMessage.GivenTaskInvalid);
        }

        var authorizationResult = await AuthorizeAsync(task);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        try
        {
            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch(Exception ex)
        {
            _logger.Error($"Internal server error {JsonConvert.SerializeObject(task)} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result> AuthorizeAsync(KanTask task)
    {
        var table = await _unitOfWork.TableRepository.GetAsync(task.TableID);
        return await _boardMembershipPolicy.Authorize(table.BoardID);
    }
}
