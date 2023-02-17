﻿using AutoMapper;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IAddTaskUseCase _addTaskUseCase;
    private readonly IUpdateTaskUseCase _updateTaskUseCase;
    private readonly IDeleteTaskUseCase _deleteTaskUseCase;
    private readonly IGetTaskUseCase _getTaskUseCase;

    public TasksController(ILogger logger,
                           IMapper mapper,
                           IAddTaskUseCase addTaskUseCase,
                           IUpdateTaskUseCase updateTaskUseCase,
                           IDeleteTaskUseCase deleteTaskUseCase,
                           IGetTaskUseCase getTaskUseCase)
    {
        _logger = logger;
        _mapper = mapper;
        _addTaskUseCase = addTaskUseCase;
        _updateTaskUseCase = updateTaskUseCase;
        _deleteTaskUseCase = deleteTaskUseCase;
        _getTaskUseCase = getTaskUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _getTaskUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return Ok(Result<KanTaskDTO>.ErrorResult(result.Errors!));
        }

        var taskDTO = _mapper.Map<KanTaskDTO>(result.Content);

        return Ok(Result<KanTaskDTO>.SuccessResult(taskDTO));    
    }

    [HttpPost]
    public async Task<IActionResult> Post(NewTaskDTO newTaskDTO)
    {
        var newTask = _mapper.Map<NewTask>(newTaskDTO);

        var result = await _addTaskUseCase.HandleAsync(newTask);

        if (!result.IsSuccess)
        {
            return Ok(Result.ErrorResult(result.Errors!));
        }

        return Ok(Result.SuccessResult());
    }

    [HttpPut]
    public async Task<IActionResult> Put(KanTaskDTO kanTaskDTO)
    {
        var task = _mapper.Map<KanTask>(kanTaskDTO);

        var result = await _updateTaskUseCase.HandleAsync(task);

        if (!result.IsSuccess)
        {
            return Ok(Result.ErrorResult(result.Errors!));
        }

        return Ok(Result.SuccessResult());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteTaskUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return Ok(Result.ErrorResult(result.Errors!));
        }

        return Ok(Result.SuccessResult());
    }
}
