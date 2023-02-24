using Kanbardoo.Application.Authorization.Policies;
using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.InvitationUseCases;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
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

namespace Kanbardoo.Application.Tests.InvitationUseCaseTests;
internal class CancelInvitationUseCaseTests
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IInvitationRepository> _invitationRepository;
    private Mock<IUserBoardRolesRepository> _userBoardRolesRepository;
    private Mock<ILogger> _logger;
    private ICancelInvitationUseCase _cancelInvitationUseCase;
    private Mock<IHttpContextAccessor> _contextAccessor;
    private Mock<IBoardOwnershipPolicy> _boardOwnershipPolicy;
    private Mock<IUserRepository> _userRepository;
    private Mock<IBoardRepository> _boardRepository;

    [SetUp]
    public void Setup()
    {
        _contextAccessor = new Mock<IHttpContextAccessor>();

        var httpContext = new DefaultHttpContext();
        _contextAccessor.Setup(e => e.HttpContext).Returns(httpContext);

        _invitationRepository = new Mock<IInvitationRepository>();
        _userBoardRolesRepository = new Mock<IUserBoardRolesRepository>();
        _userRepository = new Mock<IUserRepository>();
        _boardRepository = new Mock<IBoardRepository>();

        _boardOwnershipPolicy = new Mock<IBoardOwnershipPolicy>();

        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(e => e.InvitationRepository).Returns(_invitationRepository.Object);
        _unitOfWork.Setup(e => e.UserBoardRolesRepository).Returns(_userBoardRolesRepository.Object);
        _unitOfWork.Setup(e => e.UserRepository).Returns(_userRepository.Object);
        _unitOfWork.Setup(e => e.BoardRepository).Returns(_boardRepository.Object);

        _logger = new Mock<ILogger>();

        CancelInvitationValidator validator = new(_unitOfWork.Object);

        _cancelInvitationUseCase = new CancelInvitationUseCase(_unitOfWork.Object, _logger.Object, validator, _boardOwnershipPolicy.Object);
    }

    [Test]
    public async Task HandleAsync_ValidCancelInvitationModel_DeletesInvitation()
    {
        CancelInvitationModel model = new()
        {
            UserName = "Test",
            BoardID = 1,
        };

        KanUser kanUser = new()
        {
            ID = 1,
            UserName = "Test",
        };

        _userRepository.Setup(e => e.GetAsync(model.UserName)).ReturnsAsync(kanUser);

        Invitation invitation = new()
        {
            UserID = kanUser.ID,
            BoardID = model.BoardID,
        };

        KanBoard board = new()
        {
            ID = model.BoardID,
            Name = "Test",
        };

        _invitationRepository.Setup(e => e.DeleteAsync(invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(model.BoardID)).ReturnsAsync(board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, kanUser.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(board.ID)).ReturnsAsync(Result.SuccessResult());

        var result = await _cancelInvitationUseCase.HandleAsync(model);

        _invitationRepository.Verify(e => e.DeleteAsync(It.Is<Invitation>(e => e.BoardID == invitation.BoardID && e.UserID == invitation.UserID)), Times.Once);
    }

    [Test]
    public async Task HandleAsync_ValidInvitation_ReturnsSuccessResult()
    {
        CancelInvitationModel model = new()
        {
            UserName = "Test",
            BoardID = 1,
        };

        KanUser kanUser = new()
        {
            ID = 1,
            UserName = "Test",
        };

        _userRepository.Setup(e => e.GetAsync(model.UserName)).ReturnsAsync(kanUser);

        Invitation invitation = new()
        {
            UserID = kanUser.ID,
            BoardID = model.BoardID,
        };

        KanBoard board = new()
        {
            ID = model.BoardID,
            Name = "Test",
        };

        _invitationRepository.Setup(e => e.DeleteAsync(invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(model.BoardID)).ReturnsAsync(board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, kanUser.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(board.ID)).ReturnsAsync(Result.SuccessResult());

        var result = await _cancelInvitationUseCase.HandleAsync(model);

        Assert.NotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result, Is.TypeOf<SuccessResult>());
    }

    [Test]
    public async Task HandleAsync_NoAuthorizationToInvitation_ReturnsErrorResult()
    {
        KanUser invalidUser = new()
        {
            ID = 1,
            UserName = "Test",
        };

        CancelInvitationModel model = new()
        {
            UserName = invalidUser.UserName,
            BoardID = 1,
        };

        KanUser validUser = new()
        {
            ID = 2,
            UserName = "Test2",
        };

        _userRepository.Setup(e => e.GetAsync(model.UserName)).ReturnsAsync(validUser);

        Invitation invitation = new()
        {
            UserID = invalidUser.ID,
            BoardID = model.BoardID,
        };

        KanBoard board = new()
        {
            ID = model.BoardID,
            Name = "Test",
        };

        _invitationRepository.Setup(e => e.DeleteAsync(invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(model.BoardID)).ReturnsAsync(board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, invalidUser.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(board.ID)).ReturnsAsync(Result.SuccessResult());

        var result = await _cancelInvitationUseCase.HandleAsync(model);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Errors);
        Assert.IsNotEmpty(result.Errors.ToArray());
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result, Is.TypeOf<ErrorResult>());
    }

    [Test]
    public async Task HandleAsync_NoAuthorization_NoDelete()
    {
        KanUser invalidUser = new()
        {
            ID = 1,
            UserName = "Test",
        };

        CancelInvitationModel model = new()
        {
            UserName = invalidUser.UserName,
            BoardID = 1,
        };

        KanUser validUser = new()
        {
            ID = 2,
            UserName = "Test2",
        };

        _userRepository.Setup(e => e.GetAsync(model.UserName)).ReturnsAsync(validUser);

        Invitation invitation = new()
        {
            UserID = invalidUser.ID,
            BoardID = model.BoardID,
        };

        KanBoard board = new()
        {
            ID = model.BoardID,
            Name = "Test",
        };

        _invitationRepository.Setup(e => e.DeleteAsync(invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(model.BoardID)).ReturnsAsync(board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, invalidUser.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(board.ID)).ReturnsAsync(Result.SuccessResult());

        var result = await _cancelInvitationUseCase.HandleAsync(model);

        _invitationRepository.Verify(e => e.DeleteAsync(It.IsAny<Invitation>()), Times.Never);
    }
}
