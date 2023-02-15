using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.BoardUseCaseTests;
internal class UpdateBoardUseCaseTests
{
    private UpdateBoardUseCase _updateBoardUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private NewBoard _newBoard;

    [SetUp]
    public void Setup()
    {
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);

        _updateBoardUseCase = new UpdateBoardUseCase(_unitOfWork.Object, _logger.Object);

        _newBoard = new NewBoard();
    }

    [Test]
    public async Task HandleAsync_ValidBoard_ReturnsSuccessResult()
    {
        //Arrange
        var board = new Board()
        {
            ID = 1,
            Name = "modifiedName"
        };

        _boardRepository.Setup(e => e.UpdateAsync(board)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(board.ID)).ReturnsAsync(new Board() { ID = 1 });

        //Act
        SuccessResult successResult = await _updateBoardUseCase.HandleAsync(board) as SuccessResult;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_BoardIsNull_ReturnsErrorResult()
    {
        //Arrange
        Board board = null;

        //Act
        ErrorResult errorResult = await _updateBoardUseCase.HandleAsync(board) as ErrorResult;

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
        _boardRepository.Setup(e => e.UpdateAsync(It.IsAny<Board>())).Throws<Exception>();

        //Act
        ErrorResult errorResult = await _updateBoardUseCase.HandleAsync(It.IsAny<Board>()) as ErrorResult;

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
        ErrorResult errorResult = await _updateBoardUseCase.HandleAsync(It.IsAny<Board>()) as ErrorResult;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ValidBoard_ExecutesUpdateOnce()
    {
        var board = new Board()
        {
            ID = 1,
            Name = "test",
        };
        _boardRepository.Setup(e => e.UpdateAsync(board)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(board.ID)).ReturnsAsync(new Board() { ID = 1 });

        await _updateBoardUseCase.HandleAsync(board);

        _boardRepository.Verify(e => e.UpdateAsync(It.IsAny<Board>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidBoard_ExecutesSaveChangesAsyncOnce()
    {
        var board = new Board()
        {
            ID = 1,
            Name = "test",
        };
        _boardRepository.Setup(e => e.UpdateAsync(board)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(board.ID)).ReturnsAsync(new Board() { ID = 1 });

        await _updateBoardUseCase.HandleAsync(board);

        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }
}
