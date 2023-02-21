using Kanbardoo.Application.Authorization.Policies;
using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using Serilog;
using System.Security.Claims;

namespace Kanbardoo.Application.Tests.TableUseCaseTests;
internal class AddTableUseCaseTests
{
    private AddTableUseCase _addTableUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private NewTableValidator _newTableValidator;
    private Mock<IBoardMembershipPolicy> _boardMembershipPolicy;
    private Mock<IHttpContextAccessor> _contextAccessor;
    private Mock<IUserTablesRepository> _userTablesRepository;

    [SetUp]
    public void Setup()
    {
        _contextAccessor = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims: new[] { new Claim(KanClaimName.ID, 1.ToString()) }));
        _contextAccessor.Setup(e => e.HttpContext).Returns(httpContext);

        _tableRepository = new Mock<ITableRepository>();
        _userTablesRepository = new Mock<IUserTablesRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.TableRepository).Returns(_tableRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _newTableValidator = new NewTableValidator(_unitOfWork.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _addTableUseCase = new AddTableUseCase(_logger.Object,
                                               _unitOfWork.Object,
                                               _newTableValidator,
                                               _boardMembershipPolicy.Object,
                                               _contextAccessor.Object);
    }

    [Test]
    public async Task HandleAsync_ValidNewTable_ReturnsSuccessResult()
    {
        //Arrange
        var boardId = 1;
        NewKanTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        KanBoard board = new()
        {
            ID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(board);
        _userTablesRepository.Setup(e => e.AddAsync(It.IsAny<KanUserTable>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.UserTablesRepository).Returns(_userTablesRepository.Object);

        //Act
        SuccessResult successResult = (await _addTableUseCase.HandleAsync(newTable) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingBoard_ReturnsErrorResult()
    {
        //Arrange
        var boardId = 1;
        NewKanTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(new KanBoard());

        //Act
        ErrorResult errorResult = (await _addTableUseCase.HandleAsync(newTable) as ErrorResult)!;

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
        NewKanTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        KanBoard board = new()
        {
            ID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(board);

        //Act
        SuccessResult successResult = (await _addTableUseCase.HandleAsync(newTable) as SuccessResult)!;

        //Arrange
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidTable_ExecutesAddAsyncOnce()
    {
        //Arrange
        var boardId = 1;
        NewKanTable newTable = new()
        {
            Name = "Test",
            BoardID = boardId,
        };

        KanBoard board = new()
        {
            ID = boardId,
        };

        _boardRepository.Setup(e => e.GetAsync(boardId)).ReturnsAsync(board);

        //Act
        SuccessResult successResult = (await _addTableUseCase.HandleAsync(newTable) as SuccessResult)!;

        //Arrange
        _tableRepository.Verify(e => e.AddAsync(It.IsAny<KanTable>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NewTableIsNull_ReturnsErrorResult()
    {
        //Act
        ErrorResult errorResult = (await _addTableUseCase.HandleAsync(null!) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
