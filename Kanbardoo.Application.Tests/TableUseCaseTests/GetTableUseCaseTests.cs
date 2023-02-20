using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.TableUseCaseTests;
internal class GetTableUseCaseTests
{
    private GetTableUseCase _getTableUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private Mock<IBoardMembershipPolicy> _boardMembershipPolicy;

    [SetUp]
    public void Setup()
    {
        _tableRepository = new Mock<ITableRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.TableRepository).Returns(_tableRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);

        _boardMembershipPolicy = new Mock<IBoardMembershipPolicy>();
        _boardMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _getTableUseCase = new GetTableUseCase(_logger.Object, _unitOfWork.Object, _boardMembershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingId_ReturnsSuccessResult()
    {
        //Arrange
        int id = 1;
        KanTable table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);

        //Act
        SuccessResult<KanTable> successResult = (await _getTableUseCase.HandleAsync(id) as SuccessResult<KanTable>)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.IsNotNull(successResult.Content);
        Assert.That(successResult.Content, Is.EqualTo(table));
    }

    [Test]
    public async Task HandleAsync_NonExistingId_ReturnsErrorResult()
    {
        //Arrange
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(new KanTable());

        //Act
        ErrorResult<KanTable> errorResult = (await _getTableUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult<KanTable>)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_GetAsyncThrowsException_ReturnsErrorResult()
    {
        //Arrange
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).Throws<Exception>();

        //Act
        ErrorResult<KanTable> errorResult = (await _getTableUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult<KanTable>)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_GetAsyncNoException_ReturnsSuccessResult()
    {
        //Assert
        var tables = new List<KanTable>();
        _tableRepository.Setup(e => e.GetAsync()).ReturnsAsync(tables);

        //Act
        SuccessResult<IEnumerable<KanTable>> successResult = (await _getTableUseCase.HandleAsync() as SuccessResult<IEnumerable<KanTable>>)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.IsNotNull(successResult.Content);
        Assert.That(successResult.Content, Is.EqualTo(tables));
    }

    [Test]
    public async Task HandleAsync_GetAsyncAllThrowsException_ReturnsErrorResult()
    {
        //Arrange
        _tableRepository.Setup(e => e.GetAsync()).Throws<Exception>();

        //Act
        ErrorResult<IEnumerable<KanTable>> errorResult = (await _getTableUseCase.HandleAsync() as ErrorResult<IEnumerable<KanTable>>)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
