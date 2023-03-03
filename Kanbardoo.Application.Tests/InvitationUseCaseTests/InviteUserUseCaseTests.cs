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
using System.Reflection;
using System.Security.Claims;

namespace Kanbardoo.Application.Tests.InvitationUseCaseTests;
internal class InviteUserUseCaseTests
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IInvitationRepository> _invitationRepository;
    private Mock<IUserBoardRolesRepository> _userBoardRolesRepository;
    private Mock<ILogger> _logger;
    private IInviteUserUseCase _inviteUserUseCase;
    private Mock<IHttpContextAccessor> _contextAccessor;
    private Mock<IBoardOwnershipPolicy> _boardOwnershipPolicy;
    private Mock<IUserRepository> _userRepository;
    private Mock<IBoardRepository> _boardRepository;
    private NewInvitation _newInvitation;
    private KanUser _user1;
    private KanUser _user2;
    private Invitation _invitation;
    private KanBoard _board;

    [SetUp]
    public void Setup()
    {
        _newInvitation = new()
        {
            UserName = "Test",
            BoardID = 1,
        };

        _user1 = new()
        {
            ID = 1,
            UserName = "Test",
        };

        _user2 = new()
        {
            ID = 2,
            UserName = "Test2",
        };

        _invitation = new()
        {
            UserID = _user1.ID,
            BoardID = _newInvitation.BoardID,
        };

        _board = new()
        {
            ID = _newInvitation.BoardID,
            Name = "Test",
        };

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

        NewInvitationValidator validator = new(_unitOfWork.Object);

        _inviteUserUseCase = new InviteUserUseCase(_unitOfWork.Object, _logger.Object, validator, _boardOwnershipPolicy.Object, _contextAccessor.Object);
    }

    [Test]
    public async Task HandleAsync_ValidNewInvitation_AddsInvitation()
    {
        _userRepository.Setup(e => e.GetAsync(_newInvitation.UserName)).ReturnsAsync(_user1);

        _invitationRepository.Setup(e => e.AddAsync(_invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(_newInvitation.BoardID)).ReturnsAsync(_board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, _user1.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(_board.ID)).ReturnsAsync(Result.SuccessResult());

        var result = await _inviteUserUseCase.HandleAsync(_newInvitation);

        _invitationRepository.Verify(e => e.AddAsync(It.Is<Invitation>(e => e.BoardID == _invitation.BoardID && e.UserID == _invitation.UserID)), Times.Once);
    }

    [Test]
    public async Task HandleAsync_NoAuthorizationToBoard_ReturnsErrorResult()
    {
        _userRepository.Setup(e => e.GetAsync(_newInvitation.UserName)).ReturnsAsync(_user1);

        _invitationRepository.Setup(e => e.AddAsync(_invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(_newInvitation.BoardID)).ReturnsAsync(_board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, _user1.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(_board.ID)).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _inviteUserUseCase.HandleAsync(_newInvitation);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Errors);
        Assert.IsNotEmpty(result.Errors);
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result, Is.TypeOf<ErrorResult>());
    }

    [Test]
    public async Task HandleAsync_NoAuthorizationToBoard_NoAdd()
    {
        _userRepository.Setup(e => e.GetAsync(_newInvitation.UserName)).ReturnsAsync(_user1);

        _invitationRepository.Setup(e => e.AddAsync(_invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(_newInvitation.BoardID)).ReturnsAsync(_board);
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, _user1.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(_board.ID)).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _inviteUserUseCase.HandleAsync(_newInvitation);

        _invitationRepository.Verify(e => e.AddAsync(It.IsAny<Invitation>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_InvalidBoard_NoAdd()
    {
        _userRepository.Setup(e => e.GetAsync(_newInvitation.UserName)).ReturnsAsync(_user1);

        _invitationRepository.Setup(e => e.AddAsync(_invitation)).Returns(Task.CompletedTask);
        _boardRepository.Setup(e => e.GetAsync(_newInvitation.BoardID)).ReturnsAsync(new KanBoard());
        ClaimsPrincipal user = new(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, _user1.ID.ToString()) }));
        _contextAccessor.Object.HttpContext!.User = user;
        _boardOwnershipPolicy.Setup(e => e.AuthorizeAsync(_board.ID)).ReturnsAsync(Result.SuccessResult());

        var result = await _inviteUserUseCase.HandleAsync(_newInvitation);

        _invitationRepository.Verify(e => e.AddAsync(It.IsAny<Invitation>()), Times.Never);
    }
}
