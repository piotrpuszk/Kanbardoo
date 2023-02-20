using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.BoardUseCaseTests;
internal class UpdateBoardUseCaseTests
{
    private UpdateBoardUseCase _updateBoardUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private NewKanBoard _newBoard;
    private BoardToUpdateValidator _boardToUpdateValidator;
    private Mock<IBoardMembershipPolicy> _boardMembershipPolicy;

    [SetUp]
    public void Setup()
    {
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _boardToUpdateValidator = new BoardToUpdateValidator(_unitOfWork.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _updateBoardUseCase = new UpdateBoardUseCase(_unitOfWork.Object, _logger.Object, _boardToUpdateValidator, _boardMembershipPolicy.Object);

        _newBoard = new NewKanBoard();
    }

    [Test]
    public async Task HandleAsync_ValidBoard_ReturnsSuccessResult()
    {
        //Arrange
        var board = new KanBoard()
        {
            ID = 1,
            Name = "modifiedName",
            StatusID = 1,
            OwnerID = 1,
            CreationDate = new DateTime(2000, 1, 1, 0, 0, 0),
        };

        _boardRepository.Setup(e => e.UpdateAsync(board)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(board.ID)).ReturnsAsync(new KanBoard() { ID = 1 });

        //Act
        SuccessResult successResult = (await _updateBoardUseCase.HandleAsync(board) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_BoardIsNull_ReturnsErrorResult()
    {
        //Arrange
        KanBoard board = null!;

        //Act
        ErrorResult errorResult = (await _updateBoardUseCase.HandleAsync(board) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_UpdateThrowsException_ReturnsErrorResult()
    {
        //Arrange
        _boardRepository.Setup(e => e.UpdateAsync(It.IsAny<KanBoard>())).Throws<Exception>();

        //Act
        ErrorResult errorResult = (await _updateBoardUseCase.HandleAsync(It.IsAny<KanBoard>()) as ErrorResult)!;

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
        _unitOfWork.Setup(e => e.SaveChangesAsync()).Throws<Exception>();

        //Act
        ErrorResult errorResult = (await _updateBoardUseCase.HandleAsync(It.IsAny<KanBoard>()) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ValidBoard_ExecutesUpdateOnce()
    {
        var board = new KanBoard()
        {
            ID = 1,
            Name = "test",
            StatusID = 1,
            OwnerID = 1,
            CreationDate = new DateTime(2000, 1, 1, 0, 0, 0),
        };
        _boardRepository.Setup(e => e.UpdateAsync(board)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(board.ID)).ReturnsAsync(new KanBoard() { ID = 1 });

        await _updateBoardUseCase.HandleAsync(board);

        _boardRepository.Verify(e => e.UpdateAsync(It.IsAny<KanBoard>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidBoard_ExecutesSaveChangesAsyncOnce()
    {
        var board = new KanBoard()
        {
            ID = 1,
            Name = "test",
            StatusID = 1,
            OwnerID = 1,
            CreationDate = new DateTime(2000, 1, 1, 0, 0, 0),
        };
        _boardRepository.Setup(e => e.UpdateAsync(board)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(board.ID)).ReturnsAsync(new KanBoard() { ID = 1 });

        await _updateBoardUseCase.HandleAsync(board);

        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }
}
