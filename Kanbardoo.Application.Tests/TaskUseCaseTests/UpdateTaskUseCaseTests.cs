using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TaskUseCaseTests;
internal class UpdateTaskUseCaseTests
{
    private UpdateTaskUseCase _updateTaskUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<ITaskRepository> _taskRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ITaskStatusRepository> _taskStatusRepository;
    private Mock<ILogger> _logger;
    private KanTaskValidator _taskToUpdateValidator;
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
        _taskToUpdateValidator = new KanTaskValidator(_unitOfWork.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _updateTaskUseCase = new UpdateTaskUseCase(_logger.Object, _unitOfWork.Object, _taskToUpdateValidator, _boardMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ValidTask_ReturnsSuccessResult()
    {
        //Arrange
        var task = new KanTask()
        {
            ID = 1,
            TableID = 1,
            Name = "Test",
            StatusID = 1,
            AssigneeID = 1,
        };

        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _userRepository.Setup(e => e.GetAsync(task.AssigneeID)).ReturnsAsync(new KanUser { ID = task.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(task.StatusID)).ReturnsAsync(new KanTaskStatus { ID = task.StatusID });
        _taskRepository.Setup(e => e.UpdateAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);
        _tableRepository.Setup(e => e.GetAsync(task.TableID)).ReturnsAsync(new KanTable { ID = task.TableID, BoardID = 1, });

        //Act
        SuccessResult successResult = (await _updateTaskUseCase.HandleAsync(task) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_InvalidTaskName_ReturnsErrorResult()
    {
        //Arrange
        var task = new KanTask()
        {
            ID = 1,
            TableID = 1,
            Name = "",
            StatusID = 1,
            AssigneeID = 1,
        };

        _userRepository.Setup(e => e.GetAsync(task.AssigneeID)).ReturnsAsync(new KanUser { ID = task.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(task.StatusID)).ReturnsAsync(new KanTaskStatus { ID = task.StatusID });
        _taskRepository.Setup(e => e.UpdateAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);

        //Act
        ErrorResult errorResult = (await _updateTaskUseCase.HandleAsync(task) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
    }

    [Test]
    public async Task HandleAsync_Null_ReturnsErrorResult()
    {
        //Arrange
        KanTask task = null!;

        //Act
        ErrorResult errorResult = (await _updateTaskUseCase.HandleAsync(task) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
    }

    [Test]
    public async Task HandleAsync_ValidTask_ExecutesUpdateAsyncOnce()
    {
        //Arrange
        var task = new KanTask()
        {
            ID = 1,
            TableID = 1,
            Name = "Test",
            StatusID = 1,
            AssigneeID = 1,
        };

        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _userRepository.Setup(e => e.GetAsync(task.AssigneeID)).ReturnsAsync(new KanUser { ID = task.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(task.StatusID)).ReturnsAsync(new KanTaskStatus { ID = task.StatusID });
        _taskRepository.Setup(e => e.UpdateAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);
        _tableRepository.Setup(e => e.GetAsync(task.TableID)).ReturnsAsync(new KanTable { ID = task.TableID, BoardID = 1, });

        //Act
        SuccessResult successResult = (await _updateTaskUseCase.HandleAsync(task) as SuccessResult)!;

        //Assert
        _taskRepository.Verify(e => e.UpdateAsync(It.IsAny<KanTask>()), Times.Once);
        _taskRepository.Verify(e => e.UpdateAsync(task), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidTask_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        var task = new KanTask()
        {
            ID = 1,
            TableID = 1,
            Name = "Test",
            StatusID = 1,
            AssigneeID = 1,
        };

        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _userRepository.Setup(e => e.GetAsync(task.AssigneeID)).ReturnsAsync(new KanUser { ID = task.AssigneeID });
        _taskStatusRepository.Setup(e => e.GetAsync(task.StatusID)).ReturnsAsync(new KanTaskStatus { ID = task.StatusID });
        _taskRepository.Setup(e => e.UpdateAsync(It.IsAny<KanTask>())).Returns(Task.CompletedTask);
        _tableRepository.Setup(e => e.GetAsync(task.TableID)).ReturnsAsync(new KanTable { ID = task.TableID, BoardID = 1, });

        //Act
        SuccessResult successResult = (await _updateTaskUseCase.HandleAsync(task) as SuccessResult)!;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }
}
