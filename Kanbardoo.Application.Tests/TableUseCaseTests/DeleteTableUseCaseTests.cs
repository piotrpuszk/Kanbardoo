using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    private TableIDToDelete _tableIDToDelete;
    private Mock<ITableMembershipPolicy> _tableMembershipPolicy;
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
        _tableIDToDelete = new TableIDToDelete(_unitOfWork.Object);

        _tableMembershipPolicy = new Mock<ITableMembershipPolicy>();
        _tableMembershipPolicy.Setup(e => e.Authorize(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        _deleteTableUseCase = new DeleteTableUseCase(_logger.Object,
                                                     _unitOfWork.Object,
                                                     _tableIDToDelete,
                                                     _tableMembershipPolicy.Object,
                                                     _contextAccessor.Object);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ReturnsSuccessResult()
    {
        //Arrange
        int id = 1;
        KanTable table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);
        _userTablesRepository.Setup(e => e.AddAsync(It.IsAny<KanUserTable>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.UserTablesRepository).Returns(_userTablesRepository.Object);

        //Act
        SuccessResult successResult = (await _deleteTableUseCase.HandleAsync(id) as SuccessResult)!;

        //Assert
        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ExecutesSaveChangesAsyncAtLeastOnce()
    {
        //Arrange
        int id = 1;
        KanTable table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);
        _userTablesRepository.Setup(e => e.AddAsync(It.IsAny<KanUserTable>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(e => e.UserTablesRepository).Returns(_userTablesRepository.Object);

        //Act
        SuccessResult successResult = (await _deleteTableUseCase.HandleAsync(id) as SuccessResult)!;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task HandleAsync_ExistingTableId_ExecutesDeleteAsyncOnce()
    {
        //Arrange
        int id = 1;
        KanTable table = new()
        {
            ID = id,
        };

        _tableRepository.Setup(e => e.GetAsync(id)).ReturnsAsync(table);

        //Act
        SuccessResult successResult = (await _deleteTableUseCase.HandleAsync(id) as SuccessResult)!;

        //Assert
        _tableRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_ReturnsErrorResult()
    {
        //Arrange
        int nonExistingId = 2;
        _tableRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new KanTable());

        //Act
        ErrorResult errorResult = (await _deleteTableUseCase.HandleAsync(nonExistingId) as ErrorResult)!;

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
        _tableRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new KanTable());

        //Act
        ErrorResult errorResult = (await _deleteTableUseCase.HandleAsync(nonExistingId) as ErrorResult)!;

        //Assert
        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task HandleAsync_NonExistingId_NoExecuteDeleteAsync()
    {
        //Arrange
        int nonExistingId = 2;
        _tableRepository.Setup(e => e.GetAsync(nonExistingId)).ReturnsAsync(new KanTable());

        //Act
        ErrorResult errorResult = (await _deleteTableUseCase.HandleAsync(nonExistingId) as ErrorResult)!;

        //Assert
        _tableRepository.Verify(e => e.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_DeleteAsyncThrowsException_ReturnsErrorResult()
    {
        //Arrange
        KanTable table = new()
        {
            ID = 1,
        };

        _tableRepository.Setup(e => e.DeleteAsync(It.IsAny<int>())).Throws<Exception>();
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(table);

        //Act
        ErrorResult errorResult = (await _deleteTableUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult)!;

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
        KanTable table = new()
        {
            ID = 1,
        };

        _unitOfWork.Setup(e => e.SaveChangesAsync()).Throws<Exception>();
        _tableRepository.Setup(e => e.GetAsync(It.IsAny<int>())).ReturnsAsync(table);

        //Act
        ErrorResult errorResult = (await _deleteTableUseCase.HandleAsync(It.IsAny<int>()) as ErrorResult)!;

        //Assert
        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
