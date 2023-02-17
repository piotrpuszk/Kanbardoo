using Kanbardoo.Application.Results;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TableUseCaseTests;
internal class AddTableUseCaseTests
{
    private AddTableUseCase _addTableUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private NewTableValidator _newTableValidator;

    [SetUp]
    public void Setup()
    {
        _tableRepository = new Mock<ITableRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.TableRepository).Returns(_tableRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _newTableValidator = new NewTableValidator(_unitOfWork.Object);

        _addTableUseCase = new AddTableUseCase(_logger.Object, _unitOfWork.Object, _newTableValidator);
    }

    [Test]
    public async Task HandleAsync_ValidNewTable_ReturnsSuccessResult()
    {
        //Arrange
        var boardId = 1;
        NewTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        Board board = new()
        {
            ID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(board);

        //Act
        SuccessResult successResult = await _addTableUseCase.HandleAsync(newTable) as SuccessResult;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingBoard_ReturnsErrorResult()
    {
        //Arrange
        var boardId = 1;
        NewTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(new Board());

        //Act
        ErrorResult errorResult = await _addTableUseCase.HandleAsync(newTable) as ErrorResult;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ValidTable_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        var boardId = 1;
        NewTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        Board board = new()
        {
            ID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(board);

        //Act
        SuccessResult successResult = await _addTableUseCase.HandleAsync(newTable) as SuccessResult;

        //Arrange
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidTable_ExecutesAddAsyncOnce()
    {
        //Arrange
        var boardId = 1;
        NewTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        Board board = new()
        {
            ID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(board);

        //Act
        SuccessResult successResult = await _addTableUseCase.HandleAsync(newTable) as SuccessResult;

        //Arrange
        _tableRepository.Verify(e => e.AddAsync(It.IsAny<Table>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NewTableIsNull_ReturnsErrorResult()
    {
        //Act
        ErrorResult errorResult = await _addTableUseCase.HandleAsync(null) as ErrorResult;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
