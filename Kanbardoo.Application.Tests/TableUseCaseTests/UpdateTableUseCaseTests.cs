using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TableUseCaseTests;
internal class UpdateTableUseCaseTests
{
    private UpdateTableUseCase _updateTableUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private TableToUpdateValidator _tableToUpdateValidator;
    private Mock<ITableMembershipPolicy> _tableMembershipPolicy;

    [SetUp]
    public void Setup()
    {
        _tableRepository = new Mock<ITableRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.TableRepository).Returns(_tableRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _tableToUpdateValidator = new TableToUpdateValidator(_unitOfWork.Object);

        _tableMembershipPolicy = new Mock<ITableMembershipPolicy>();
        _tableMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _updateTableUseCase = new UpdateTableUseCase(_logger.Object, _unitOfWork.Object, _tableToUpdateValidator, _tableMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ValidTable_ReturnsSuccessResult()
    {
        //Arrange
        var table = new KanTable()
        {
            ID = 1,
            BoardID = 1,
            Name = "Test",
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _tableRepository.Setup(e => e.UpdateAsync(table)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _boardRepository.Setup(e => e.GetAsync(table.BoardID)).ReturnsAsync(new KanBoard() { Name = "board", ID = 1 });
        _tableRepository.Setup(e => e.GetAsync(table.ID)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = (await _updateTableUseCase.HandleAsync(table) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_InvalidTableName_ReturnsErrorResult()
    {
        //Arrange
        var table = new KanTable()
        {
            BoardID = 1,
            Name = "",
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _tableRepository.Setup(e => e.UpdateAsync(table)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _boardRepository.Setup(e => e.GetAsync(table.BoardID)).ReturnsAsync(new KanBoard() { Name = "board", ID = 1 });
        _tableRepository.Setup(e => e.GetAsync(table.ID)).ReturnsAsync(table);

        //Act
        ErrorResult errorResult = (await _updateTableUseCase.HandleAsync(table) as ErrorResult)!;

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
        KanTable table = null!;

        _tableRepository.Setup(e => e.UpdateAsync(table)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);

        //Act
        ErrorResult errorResult = (await _updateTableUseCase.HandleAsync(table) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
    }

    [Test]
    public async Task HandleAsync_ValidTable_ExecutesUpdateAsyncOnce()
    {
        //Arrange
        var table = new KanTable()
        {
            ID = 1,
            BoardID = 1,
            Name = "Test",
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _tableRepository.Setup(e => e.UpdateAsync(table)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _boardRepository.Setup(e => e.GetAsync(table.BoardID)).ReturnsAsync(new KanBoard() { Name = "board", ID = 1 });
        _tableRepository.Setup(e => e.GetAsync(table.ID)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = (await _updateTableUseCase.HandleAsync(table) as SuccessResult)!;

        //Assert
        _tableRepository.Verify(e => e.UpdateAsync(It.IsAny<KanTable>()), Times.Once);
        _tableRepository.Verify(e => e.UpdateAsync(table), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidTable_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        var table = new KanTable()
        {
            ID = 1,
            BoardID = 1,
            Name = "test",
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _tableRepository.Setup(e => e.UpdateAsync(table)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _boardRepository.Setup(e => e.GetAsync(table.BoardID)).ReturnsAsync(new KanBoard() { Name = "board", ID = 1 });
        _tableRepository.Setup(e => e.GetAsync(table.ID)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = (await _updateTableUseCase.HandleAsync(table) as SuccessResult)!;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }
}
