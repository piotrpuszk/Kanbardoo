﻿using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TaskUseCases;
public class GetTaskUseCase : IGetTaskUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetTaskUseCase(ILogger logger,
                           IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<KanTask>> HandleAsync(int id)
    {
        KanTask task = new();
        try
        {
            task = await _unitOfWork.TaskRepository.GetAsync(id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error {id} \n\n {ex}");
            return ErrorResult<KanTask>.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }

        if (!task.Exists())
        {
            _logger.Error($"A task with the give id {id} does not exist");
            return ErrorResult<KanTask>.ErrorResult(ErrorMessage.TaskWithIDNotExist);
        }

        return Result<KanTask>.SuccessResult(task);
    }
}