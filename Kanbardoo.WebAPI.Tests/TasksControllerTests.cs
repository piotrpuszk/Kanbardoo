using AutoMapper;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.Controllers;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using System.Net;

namespace Kanbardoo.WebAPI.Tests;
internal class TasksControllerTests
{
    private Mock<ILogger> _logger;
    private Mock<IAddTaskUseCase> _addTaskUseCase;
    private Mock<IUpdateTaskUseCase> _updateTaskUseCase;
    private Mock<IDeleteTaskUseCase> _deleteTaskUseCase;
    private Mock<IGetTaskUseCase> _getTaskUseCase;
    private Mock<IMapper> _mapper;
    private TasksController _tasksController;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger>();
        _addTaskUseCase = new Mock<IAddTaskUseCase>();
        _updateTaskUseCase = new Mock<IUpdateTaskUseCase>();
        _deleteTaskUseCase = new Mock<IDeleteTaskUseCase>();
        _getTaskUseCase = new Mock<IGetTaskUseCase>();
        _mapper = new Mock<IMapper>();

        _tasksController = new TasksController(
            _logger.Object,
            _mapper.Object,
            _addTaskUseCase.Object,
            _updateTaskUseCase.Object,
            _deleteTaskUseCase.Object,
            _getTaskUseCase.Object
            );
    }

    [Test]
    public async Task Post_ValidNewTaskDTO_ReturnsOkWithSuccessResult()
    {
        NewTaskDTO newTaskDTO = new NewTaskDTO()
        {
            Name = "Test",
        };

        NewTask newTask = new NewTask()
        {
            Name = newTaskDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewTask>(newTaskDTO)).Returns(newTask);
        _addTaskUseCase.Setup(e => e.HandleAsync(newTask)).ReturnsAsync(Result.SuccessResult());

        var result = await _tasksController.Post(newTaskDTO) as OkObjectResult;

        Assert.IsNotNull(result);

        var successResult = result.Value as SuccessResult;

        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Post_EmptyNewTaskName_ReturnsBadRequestWithErrorResult()
    {
        NewTaskDTO newTaskDTO = new NewTaskDTO()
        {
            Name = string.Empty,
        };

        NewTask newTask = new NewTask()
        {
            Name = newTaskDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewTask>(newTaskDTO)).Returns(newTask);
        _addTaskUseCase.Setup(e => e.HandleAsync(newTask)).ReturnsAsync(Result.ErrorResult(""));

        var result = await _tasksController.Post(newTaskDTO) as BadRequestObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
    }

    [Test]
    public async Task Put_ValidTaskDTO_ReturnsOkWithSuccessResult()
    {
        KanTaskDTO taskDTO = new()
        {
            ID = 1,
            Name= "Test",
        };

        KanTask task = new()
        {
            ID = 1,
            Name = "Test",
        };

        _mapper.Setup(e => e.Map<KanTask>(taskDTO)).Returns(task);

        _updateTaskUseCase.Setup(e => e.HandleAsync(task)).ReturnsAsync(Result.SuccessResult());

        var result = await _tasksController.Put(taskDTO) as OkObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var successResult = result.Value as SuccessResult;

        Assert.NotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Put_InvalidTaskDTO_ReturnsBadRequestWithErrorResult()
    {
        KanTaskDTO taskDTO = new()
        {
            ID = default,
        };

        KanTask task = new()
        {
            ID = taskDTO.ID,
        };

        _mapper.Setup(e => e.Map<KanTask>(taskDTO)).Returns(task);
        _updateTaskUseCase.Setup(e => e.HandleAsync(task)).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _tasksController.Put(taskDTO) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
        Assert.NotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Delete_ExistingId_ReturnsOkWithSuccessResult()
    {
        _deleteTaskUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        var result = await _tasksController.Delete(It.IsAny<int>()) as OkObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var successResult = result.Value as SuccessResult;

        Assert.NotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Delete_NonExistingId_ReturnsBadRequestWithErrorResult()
    {
        _deleteTaskUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _tasksController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
        Assert.NotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task GetById_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _getTaskUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result<KanTask>.ErrorResult("Internal server error", HttpStatusCode.InternalServerError));

        var result = await _tasksController.Get(It.IsAny<int>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<KanTaskDTO>;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Post_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _addTaskUseCase.Setup(e => e.HandleAsync(It.IsAny<NewTask>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        var result = await _tasksController.Post(It.IsAny<NewTaskDTO>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Put_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _updateTaskUseCase.Setup(e => e.HandleAsync(It.IsAny<KanTask>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        var result = await _tasksController.Put(It.IsAny<KanTaskDTO>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Delete_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _deleteTaskUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        var result = await _tasksController.Delete(It.IsAny<int>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
