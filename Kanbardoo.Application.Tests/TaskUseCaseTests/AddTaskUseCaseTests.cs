using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TaskUseCaseTests;
internal class AddTaskUseCaseTests
{
    private AddTaskUseCase _addTaskUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<ITaskRepository> _taskRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ITaskStatusRepository> _taskStatusRepository;
    private Mock<ILogger> _logger;
    private NewTaskValidator _newTaskValidator;
    private Mock<IBoardMembershipPolicy> _boardMembershipPolicy;

    [SetUp]
    public void Setup()
    {
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
        _newTaskValidator = new NewTaskValidator(_unitOfWork.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _addTaskUseCase = new AddTaskUseCase(_logger.Object, _unitOfWork.Object, _newTaskValidator, _boardMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ValidNewTask_ReturnsSuccessResult()
    {
        //Arrange
        var tableId = 1;
        NewKanTask newTask = new()
        {
            Name = "Test",
            TableID = tableId,
            AssigneeID= 1,
            StatusID= 1,
        };

        KanTable table = new()
        {
            ID = tableId,
        };

        _tableRepository.Setup(e => e.GetAsync(tableId)).ReturnsAsync(table);
        _userRepository.Setup(e => e.GetAsync(newTask.AssigneeID)).ReturnsAsync(new KanUser { ID = newTask.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(newTask.StatusID)).ReturnsAsync(new KanTaskStatus { ID = newTask.StatusID });
        _taskRepository.Setup(e => e.AddAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);

        //Act
        SuccessResult successResult = (await _addTaskUseCase.HandleAsync(newTask) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingTable_ReturnsErrorResult()
    {
        //Arrange
        var tableId = 1;
        NewKanTask newTask = new()
        {
            Name = "Test",
            TableID = tableId,
        };

        _tableRepository.Setup(e => e.GetAsync(tableId)).ReturnsAsync(new KanTable());
        _userRepository.Setup(e => e.GetAsync(newTask.AssigneeID)).ReturnsAsync(new KanUser { ID = newTask.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(newTask.StatusID)).ReturnsAsync(new KanTaskStatus { ID = newTask.StatusID });

        //Act
        ErrorResult errorResult = (await _addTaskUseCase.HandleAsync(newTask) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ValidTask_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        var tableId = 1;
        NewKanTask newTask = new()
        {
            Name = "Test",
            TableID = tableId,
            AssigneeID = 1,
            StatusID = 1,
        };

        KanTable table = new()
        {
            ID = tableId,
        };

        _tableRepository.Setup(e => e.GetAsync(tableId)).ReturnsAsync(table);
        _userRepository.Setup(e => e.GetAsync(newTask.AssigneeID)).ReturnsAsync(new KanUser { ID = newTask.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(newTask.StatusID)).ReturnsAsync(new KanTaskStatus { ID = newTask.StatusID });
        _taskRepository.Setup(e => e.AddAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);

        //Act
        SuccessResult successResult = (await _addTaskUseCase.HandleAsync(newTask) as SuccessResult)!;

        //Arrange
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidTask_ExecutesAddAsyncOnce()
    {
        //Arrange
        var tableId = 1;
        NewKanTask newTask = new()
        {
            Name = "Test",
            TableID = tableId,
            AssigneeID = 1,
            StatusID = 1,
        };

        KanTable table = new()
        {
            ID = tableId,
        };

        _tableRepository.Setup(e => e.GetAsync(tableId)).ReturnsAsync(table);
        _userRepository.Setup(e => e.GetAsync(newTask.AssigneeID)).ReturnsAsync(new KanUser { ID = newTask.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(newTask.StatusID)).ReturnsAsync(new KanTaskStatus { ID = newTask.StatusID });
        _taskRepository.Setup(e => e.AddAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);

        //Act
        SuccessResult successResult = (await _addTaskUseCase.HandleAsync(newTask) as SuccessResult)!;

        //Arrange
        _taskRepository.Verify(e => e.AddAsync(It.IsAny<KanTask>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NewTaskIsNull_ReturnsErrorResult()
    {
        //Act
        ErrorResult errorResult = (await _addTaskUseCase.HandleAsync(null!) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
