using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Moq;
using Serilog;

namespace Kanbardoo.Application.Tests.BoardUseCaseTests;
internal class DeleteBoardUseCaseTests
{
    private DeleteBoardUseCase _deleteBoardUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;
    private BoardIdToDeleteValidator _boardIdToDeleteValidator;

    [SetUp]
    public void Setup()
    {
        _boardRepository = new Mock<IBoardRepository>();
        _logger = new Mock<ILogger>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(e => e.SaveChangesAsync()).ReturnsAsync(0);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);
        _boardIdToDeleteValidator = new BoardIdToDeleteValidator(_unitOfWork.Object);

        _deleteBoardUseCase = new DeleteBoardUseCase(_unitOfWork.Object, _logger.Object, _boardIdToDeleteValidator);
    }

    [Test]
    public async Task HandleAsync_ExistingBoardId_ReturnsSuccessResult()
    {
        //Arrange
        int id = 1;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board() { ID = id });
        _boardRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<SuccessResult>(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ExistingBoardId_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        int id = 1;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board() { ID = id });
        _boardRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task HandleAsync_ExistingBoardId_ExecutesDeleteAsyncOnce()
    {
        //Arrange
        int id = 1;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board() { ID = id });
        _boardRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        _boardRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NonExistingBoardId_ReturnsErrorResult()
    {
        //Arrange
        int id = 165;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board());

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var errorResult = result as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingBoardId_NoExecuteDeleteAsync()
    {
        //Arrange
        int id = 165;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board());

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        _boardRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_NonExistingBoardId_NoExecuteSaveChangesAsync()
    {
        //Arrange
        int id = 165;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board());

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task HandleAsync_DeletingThrowsError_ReturnsErrorResult()
    {
        //Arrange
        int id = 165;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board());
        _boardRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Throws<Exception>();

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var errorResult = result as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_SavingChangesThrowsError_ReturnsErrorResult()
    {
        //Arrange
        int id = 165;
        _boardRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(new Board());
        _boardRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.SaveChangesAsync()).Throws<Exception>();

        //Act
        Result result = await _deleteBoardUseCase.HandleAsync(id);

        //Assert
        Assert.IsNotNull(result);

        var errorResult = result as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
