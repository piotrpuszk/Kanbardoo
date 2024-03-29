﻿using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TaskUseCaseTests;
internal class GetTaskUseCaseTests
{
    private GetTaskUseCase _getTaskUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<ITaskRepository> _taskRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ITaskStatusRepository> _taskStatusRepository;
    private Mock<ILogger> _logger;
    private Mock<IBoardMembershipPolicy> _boardMembershipPolicy;
    private Mock<IResourceIdConverterRepository> _resourceIdConverterRepository;

    [SetUp]
    public void Setup()
    {
        _resourceIdConverterRepository = new Mock<IResourceIdConverterRepository>();
        _tableRepository = new Mock<ITableRepository>();
        _userRepository = new Mock<IUserRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _taskRepository = new Mock<ITaskRepository>();
        _taskStatusRepository = new Mock<ITaskStatusRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.TableRepository).Returns(_tableRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _unitOfWork.Setup(e => e.UserRepository).Returns(_userRepository.Object);
        _unitOfWork.Setup(e => e.TaskStatusRepository).Returns(_taskStatusRepository.Object);
        _unitOfWork.Setup(e => e.TaskRepository).Returns(_taskRepository.Object);
        _unitOfWork.Setup(e => e.ResourceIdConverterRepository).Returns(_resourceIdConverterRepository.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.AuthorizeAsync(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _getTaskUseCase = new GetTaskUseCase(_logger.Object, _unitOfWork.Object, _boardMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingId_ReturnsSuccessResult()
    {
        //Arrange
        int id = 1;
        KanTask task = new()
        {
            ID = id,
        };

        _taskRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(task);
        _tableRepository.Setup(e => e.GetAsync(task.TableID)).ReturnsAsync(new KanTable { ID = task.TableID, BoardID = 1, });

        //Act
        var successResult = await _getTaskUseCase.HandleAsync(id) as SuccessResult<KanTask>;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.IsNotNull(successResult.Content);
        Assert.That(successResult.Content, Is.EqualTo(task));
    }

    [Test]
    public async Task HandleAsync_NonExistingId_ReturnsErrorResult()
    {
        //Arrange
        _taskRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(new KanTask());

        //Act
        var errorResult = await _getTaskUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult<KanTask>;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_GetAsyncThrowsException_ReturnsErrorResult()
    {
        //Arrange
        _taskRepository.Setup(e => e.GetAsync(It.IsAny<int>())).Throws<Exception>();

        //Act
        var errorResult = await _getTaskUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult<KanTask>;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
