using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TaskUseCaseTests;
internal class DeleteTaskUseCaseTests
{
    private DeleteTaskUseCase _deleteTaskUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<ITaskRepository> _taskRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ITaskStatusRepository> _taskStatusRepository;
    private Mock<ILogger> _logger;
    private KanTaskIdToDeleteValidator _kanTaskIdToDeleteValidator;
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
        _kanTaskIdToDeleteValidator = new KanTaskIdToDeleteValidator(_unitOfWork.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _deleteTaskUseCase = new DeleteTaskUseCase(_logger.Object, _unitOfWork.Object, _kanTaskIdToDeleteValidator, _boardMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingTaskId_ReturnsSuccessResult()
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
        SuccessResult successResult = (await _deleteTaskUseCase.HandleAsync(id) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        int id = 1;
        KanTask task = new()
        {
            ID = id,
            TableID = 1,
        };

        _taskRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(task);
        _tableRepository.Setup(e => e.GetAsync(task.TableID)).ReturnsAsync(new KanTable { ID = task.TableID, BoardID = 1, });

        //Act
        SuccessResult successResult = (await _deleteTaskUseCase.HandleAsync(id) as SuccessResult)!;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task HandleAsync_ExistingTaskId_ExecutesDeleteAsyncOnce()
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
        SuccessResult successResult = (await _deleteTaskUseCase.HandleAsync(id) as SuccessResult)!;

        //Assert
        _taskRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_ReturnsErrorResult()
    {
        //Arrange
        int nonExistingId = 2;
        _taskRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new KanTask());

        //Act
        ErrorResult errorResult = (await _deleteTaskUseCase.HandleAsync(nonExistingId) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_NoExecuteSaveChangesAsync()
    {
        //Arrange
        int nonExistingId = 2;
        _taskRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new KanTask());

        //Act
        ErrorResult errorResult = (await _deleteTaskUseCase.HandleAsync(nonExistingId) as ErrorResult)!;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_NoExecuteDeleteAsync()
    {
        //Arrange
        int nonExistingId = 2;
        _taskRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new KanTask());

        //Act
        ErrorResult errorResult = (await _deleteTaskUseCase.HandleAsync(nonExistingId) as ErrorResult)!;

        //Assert
        _taskRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_DeleteAsyncThrowsException_ReturnsErrorResult()
    {
        //Arrange
        KanTask task = new()
        {
            ID = 1,
        };

        _taskRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Throws<Exception>();
        _taskRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(task);
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(new KanTable { BoardID = 1 });

        //Act
        ErrorResult errorResult = (await _deleteTaskUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_SaveChangesAsyncThrowsException_ReturnsErrorResult()
    {
        //Arrange
        KanTask task = new()
        {
            ID = 1,
        };

        _unitOfWork.Setup(e => e.SaveChangesAsync()).Throws<Exception>();
        _taskRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(task);
        _tableRepository.Setup(e => e.GetAsync(task.TableID)).ReturnsAsync(new KanTable { ID = task.TableID, BoardID = 1, });

        //Act
        ErrorResult errorResult = (await _deleteTaskUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
