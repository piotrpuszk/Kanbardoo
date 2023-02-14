using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests;
internal class GetBoardUseCaseTests
{
    private GetBoardUseCase _getBoardUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;

    [SetUp]
    public void Setup()
    {
        _boardRepository = new Mock<IBoardRepository>();
        _logger = new Mock<ILogger>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);

        _getBoardUseCase = new GetBoardUseCase(_unitOfWork.Object, _logger.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingId_ReturnsCorrespondingBoard()
    {
        //Arrange
        int id = 1;
        var correspondingBoard = new Board { ID = id };
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(correspondingBoard);

        //Act
        Result<Board> result = await _getBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var successResult = result as SuccessResult<Board>;

        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.IsNotNull(successResult.Content);
        Assert.That(correspondingBoard, Is.EqualTo(result.Content));
    }

    [Test]
    public async Task HandleAsync_NonExistingId_ReturnsErrorResult()
    {
        //Arrange
        int id = 1;
        var defaultBoard = new Board();
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(defaultBoard);

        //Act
        Result<Board> result = await _getBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var errorResult = result as ErrorResult<Board>;
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_AnyId_ExecutesGetAsyncOnce()
    {
        //Arrange
        int id = default;
        var defaultBoard = new Board();
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(defaultBoard);

        //Act
        Result<Board> result = await _getBoardUseCase.HandleAsync(id);

        //Assert
        _boardRepository.Verify(e => e.GetAsync(id), Times.Once);

        //Arrange
        id = 1;
        var board = new Board() { ID = id };
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(board);

        //Act
        Result<Board> secondResult = await _getBoardUseCase.HandleAsync(id);

        //Assert
        _boardRepository.Verify(e => e.GetAsync(id), Times.Once);
    }

    [Test]
    public async Task HandleAsync_AnyIdThrowsException_ReturnsErrorResult()
    {
        //Arrange
        _boardRepository.Setup(e => e.GetAsync(It.IsAny<int>())).Throws<Exception>();

        //Act
        ErrorResult<Board> result = await _getBoardUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult<Board>;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Errors);
        Assert.IsNotEmpty(result.Errors);
        Assert.IsFalse(result.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ValidFilters_ReturnsSuccessResult()
    {
        //Arrange
        BoardFilters boardFilters = new()
        {
            BoardName = It.IsAny<string>(),
            OrderByClauses = new List<OrderByClause<Board>>() 
            {
                new()
                {
                     ColumnName = nameof(Board.Name),
                },
                new()
                {
                     ColumnName = nameof(Board.ID),
                },
                new()
                {
                     ColumnName = nameof(Board.StatusID),
                },
                new()
                {
                     ColumnName = nameof(Board.CreationDate),
                },
                new()
                {
                     ColumnName = nameof(Board.StartDate),
                },
                new()
                {
                     ColumnName = nameof(Board.FinishDate),
                },
            },
        };
        var returnValue = new List<Board>()
        {
            new Board(),
            new Board(),
            new Board(),
        };
        _boardRepository.Setup(e => e.GetAsync(boardFilters)).ReturnsAsync(returnValue);

        //Act
        SuccessResult<IEnumerable<Board>> result = await _getBoardUseCase.HandleAsync(boardFilters) as SuccessResult<IEnumerable<Board>>;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Content);
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Content.Count(), Is.EqualTo(returnValue.Count));
    }

    [Test]
    public async Task HandleAsync_NonExistingOrderByColumn_ReturnsErrorResult()
    {
        //Arrange
        BoardFilters boardFilters = new()
        {
            OrderByClauses = new List<OrderByClause<Board>>()
            {
                new OrderByClause<Board>()
                {
                    ColumnName = "NonExistingColumn",
                }
            }
        };

        //Act
        ErrorResult<IEnumerable<Board>> result = await _getBoardUseCase.HandleAsync(boardFilters) as ErrorResult<IEnumerable<Board>>;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Errors);
        Assert.IsNotEmpty(result.Errors);
        Assert.IsFalse(result.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_GetAsyncThrowsError_ReturnsErrorResult()
    {
        //Arrange
        BoardFilters boardFilters = new()
        {
            OrderByClauses = new List<OrderByClause<Board>>()
            {
                new OrderByClause<Board>()
                {
                    ColumnName = nameof(Board.Name),
                }
            }
        };

        _boardRepository.Setup(e => e.GetAsync(boardFilters)).Throws<Exception>();

        //Act
        ErrorResult<IEnumerable<Board>> result = await _getBoardUseCase.HandleAsync(boardFilters) as ErrorResult<IEnumerable<Board>>;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Errors);
        Assert.IsNotEmpty(result.Errors);
        Assert.IsFalse(result.IsSuccess);
    }
}
