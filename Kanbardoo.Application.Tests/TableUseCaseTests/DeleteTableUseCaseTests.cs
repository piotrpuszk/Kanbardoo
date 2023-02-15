using Kanbardoo.Application.Results;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Application.Tests.TableUseCaseTests;
internal class DeleteTableUseCaseTests
{
    private DeleteTableUseCase _deleteTableUseCase;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<ITableRepository> _tableRepository;
    private Mock<IBoardRepository> _boardRepository;
    private Mock<ILogger> _logger;

    [SetUp]
    public void Setup()
    {
        _tableRepository = new Mock<ITableRepository>();
        _boardRepository = new Mock<IBoardRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger>();

        _unitOfWork.Setup(e => e.TableRepository).Returns(_tableRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);

        _deleteTableUseCase = new DeleteTableUseCase(_logger.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ReturnsSuccessResult()
    {
        //Arrange
        int id = 1;
        Table table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = await _deleteTableUseCase.HandleAsync(id) as SuccessResult;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        int id = 1;
        Table table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = await _deleteTableUseCase.HandleAsync(id) as SuccessResult;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ExecutesDeleteAsyncOnce()
    {
        //Arrange
        int id = 1;
        Table table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = await _deleteTableUseCase.HandleAsync(id) as SuccessResult;

        //Assert
        _tableRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_ReturnsErrorResult()
    {
        //Arrange
        int nonExistingId = 2;
        _tableRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new Table());

        //Act
        ErrorResult errorResult = await _deleteTableUseCase.HandleAsync(nonExistingId) as ErrorResult;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_NoExecuteSaveChangesAsync()
    {
        //Arrange
        int nonExistingId = 2;
        _tableRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new Table());

        //Act
        ErrorResult errorResult = await _deleteTableUseCase.HandleAsync(nonExistingId) as ErrorResult;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_NoExecuteDeleteAsync()
    {
        //Arrange
        int nonExistingId = 2;
        _tableRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new Table());

        //Act
        ErrorResult errorResult = await _deleteTableUseCase.HandleAsync(nonExistingId) as ErrorResult;

        //Assert
        _tableRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_DeleteAsyncThrowsException_ReturnsErrorResult()
    {
        //Arrange
        Table table = new()
        {
            ID = 1,
        };

        _tableRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Throws<Exception>();
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(table);

        //Act
        ErrorResult errorResult = await _deleteTableUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult;

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
        Table table = new()
        {
            ID = 1,
        };

        _unitOfWork.Setup(e => e.SaveChangesAsync()).Throws<Exception>();
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(table);

        //Act
        ErrorResult errorResult = await _deleteTableUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
