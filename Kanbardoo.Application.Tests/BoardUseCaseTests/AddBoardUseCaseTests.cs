using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.BoardUseCaseTests;
internal class AddBoardUseCaseTests
{
    private AddBoardUseCase _addBoardUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private NewKanBoard _newBoard;
    private NewBoardValidator _newBoardValidator;

    [SetUp]
    public void Setup()
    {
        _newBoardValidator = new NewBoardValidator();
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);

        _addBoardUseCase = new AddBoardUseCase(_unitOfWork.Object, _logger.Object, _newBoardValidator);

        _newBoard = new NewKanBoard();
    }

    [Test]
    public async Task HandleAsync_ValidNewBoard_ReturnsEmptySuccessResult()
    {
        //Arrange
        _boardRepository.Setup(e => e.AddAsync(It.IsAny<KanBoard>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(1);
        var boardName = "Test";
        _newBoard = new()
        {
            Name = boardName,
        };

        //Act
        Result result = await _addBoardUseCase.HandleAsync(_newBoard);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsInstanceOf<SuccessResult>(result);
    }

    [Test]
    public async Task HandleAsync_AddAsyncException_ReturnsCorrectErrorResult()
    {
        _boardRepository.Setup(e => e.AddAsync(It.IsAny<KanBoard>())).Throws<Exception>();
        var boardName = "Test";
        _newBoard = new()
        {
            Name = boardName,
        };

        Result result = await _addBoardUseCase.HandleAsync(_newBoard);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<ErrorResult>(result);

        var errorResult = result as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_SaveChangesException_ReturnsCorrectErrorResult()
    {
        //Arrange
        _unitOfWork.Setup(e => e.SaveChangesAsync()).Throws<Exception>();
        var boardName = "Test";
        _newBoard = new()
        {
            Name = boardName,
        };

        //Act
        Result result = await _addBoardUseCase.HandleAsync(_newBoard);

        //Assert
        Assert.IsNotNull(result);

        var errorResult = result as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_AddingBoardSuccessfully_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        var boardName = "Test";
        _newBoard = new()
        {
            Name = boardName,
        };

        //Act
        Result result = await _addBoardUseCase.HandleAsync(_newBoard);

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task HandleAsync_NewBoardIsNull_ReturnsErrorResult()
    {
        _newBoard = null!;

        ErrorResult errorResult = (await _addBoardUseCase.HandleAsync(_newBoard) as ErrorResult)!;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.That(errorResult.Errors.First(), Is.EqualTo("A new board is invalid"));
    }

    [Test]
    public async Task HandleAsync_NameIsNull_ReturnsErrorResult()
    {
        _newBoard = new NewKanBoard()
        {
            Name = null!,
        };
        ErrorResult errorResult = (await _addBoardUseCase.HandleAsync(_newBoard) as ErrorResult)!;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
