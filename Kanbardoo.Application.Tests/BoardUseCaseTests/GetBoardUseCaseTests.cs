using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.BoardUseCaseTests;
internal class GetBoardUseCaseTests
{
    private GetBoardUseCase _getBoardUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private BoardFiltersValidator _boardFiltersValidator;
    private Mock<IBoardMembershipPolicy> _boardMembershipPolicy;

    [SetUp]
    public void Setup()
    {
        _boardFiltersValidator = new BoardFiltersValidator();
        _boardRepository = new Mock<IBoardRepository>();
        _logger = new Mock<ILogger>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.AuthorizeAsync(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _getBoardUseCase = new GetBoardUseCase(_unitOfWork.Object, _logger.Object, _boardFiltersValidator, _boardMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingId_ReturnsCorrespondingBoard()
    {
        //Arrange
        int id = 1;
        var correspondingBoard = new KanBoard { ID = id };
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(correspondingBoard);

        //Act
        Result<KanBoard> result = await _getBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var successResult = result as SuccessResult<KanBoard>;

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
        var defaultBoard = new KanBoard();
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(defaultBoard);

        //Act
        Result<KanBoard> result = await _getBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var errorResult = result as ErrorResult<KanBoard>;
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingBoardId_ExecutesGetAsyncOnce()
    {
        //Arrange
        int id = default;
        var defaultBoard = new KanBoard();
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(defaultBoard);

        //Act
        Result<KanBoard> result = await _getBoardUseCase.HandleAsync(id);

        //Assert
        _boardRepository.Verify(e => e.GetAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ExistingBoardId_ExecutesGetAsyncOnce()
    {

        //Arrange
        int id = 1;
        var board = new KanBoard() { ID = id };
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(board);

        //Act
        Result<KanBoard> secondResult = await _getBoardUseCase.HandleAsync(id);

        //Assert
        _boardRepository.Verify(e => e.GetAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_AnyIdThrowsException_ReturnsErrorResult()
    {
        //Arrange
        _boardRepository.Setup(e => e.GetAsync(It.IsAny<int>())).Throws<Exception>();

        //Act
        ErrorResult<KanBoard> result = (await _getBoardUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult<KanBoard>)!;

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
        KanBoardFilters boardFilters = new()
        {
            BoardName = It.IsAny<string>(),
            OrderByClauses = new List<OrderByClause<KanBoard>>()
            {
                new()
                {
                     ColumnName = nameof(KanBoard.Name),
                },
                new()
                {
                     ColumnName = nameof(KanBoard.ID),
                },
                new()
                {
                     ColumnName = nameof(KanBoard.StatusID),
                },
                new()
                {
                     ColumnName = nameof(KanBoard.CreationDate),
                },
                new()
                {
                     ColumnName = nameof(KanBoard.StartDate),
                },
                new()
                {
                     ColumnName = nameof(KanBoard.FinishDate),
                },
            },
        };
        var returnValue = new List<KanBoard>()
        {
            new KanBoard(),
            new KanBoard(),
            new KanBoard(),
        };
        _boardRepository.Setup(e => e.GetAsync(boardFilters)).ReturnsAsync(returnValue);

        //Act
        SuccessResult<IEnumerable<KanBoard>> result = (await _getBoardUseCase.HandleAsync(boardFilters) as SuccessResult<IEnumerable<KanBoard>>)!;

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
        KanBoardFilters boardFilters = new()
        {
            OrderByClauses = new List<OrderByClause<KanBoard>>()
            {
                new OrderByClause<KanBoard>()
                {
                    ColumnName = "NonExistingColumn",
                }
            }
        };

        //Act
        ErrorResult<IEnumerable<KanBoard>> result = (await _getBoardUseCase.HandleAsync(boardFilters) as ErrorResult<IEnumerable<KanBoard>>)!;

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
        KanBoardFilters boardFilters = new()
        {
            OrderByClauses = new List<OrderByClause<KanBoard>>()
            {
                new OrderByClause<KanBoard>()
                {
                    ColumnName = nameof(KanBoard.Name),
                }
            }
        };

        _boardRepository.Setup(e => e.GetAsync(boardFilters)).Throws<Exception>();

        //Act
        ErrorResult<IEnumerable<KanBoard>> result = (await _getBoardUseCase.HandleAsync(boardFilters) as ErrorResult<IEnumerable<KanBoard>>)!;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Errors);
        Assert.IsNotEmpty(result.Errors);
        Assert.IsFalse(result.IsSuccess);
    }
}
