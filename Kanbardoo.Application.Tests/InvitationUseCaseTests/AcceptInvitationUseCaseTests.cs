
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.InvitationUseCases;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using Serilog;
using System.Security.Claims;

namespace Kanbardoo.Application.Tests.InvitationUseCaseTests;
internal class AcceptInvitationUseCaseTests
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IInvitationRepository> _invitationRepository;
    private Mock<IUserBoardRolesRepository> _userBoardRolesRepository;
    private Mock<ILogger> _logger;
    private IAcceptInvitationUseCase _acceptInvitationUseCase;
    private Mock<IHttpContextAccessor> _contextAccessor;

    [SetUp]
    public void Setup()
    {
        _contextAccessor = new Mock<IHttpContextAccessor>();
        _invitationRepository = new Mock<IInvitationRepository>();
        _userBoardRolesRepository = new Mock<IUserBoardRolesRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(e => e.InvitationRepository).Returns(_invitationRepository.Object);
        _unitOfWork.Setup(e => e.UserBoardRolesRepository).Returns(_userBoardRolesRepository.Object);
        _logger = new Mock<ILogger>();

        AcceptInvitationValidator validator = new(_unitOfWork.Object, _contextAccessor.Object);

        _acceptInvitationUseCase = new AcceptInvitationUseCase(_unitOfWork.Object, _logger.Object, validator);
    }

    [Test]
    public async Task AcceptInvitation_ValidInvitation_DeletesThisInvitation()
    {
        AcceptInvitation acceptInvitation = new()
        {
            ID = 1,
        };

        Invitation invitation = new()
        {
            ID = acceptInvitation.ID,
            UserID = 1,
            BoardID = 1,
        };

        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, invitation.UserID.ToString()) }));
        context.User = user;
        _contextAccessor.Setup(e => e.HttpContext).Returns(context);

        _invitationRepository.Setup(e => e.DeleteAsync(acceptInvitation.ID)).Returns(Task.CompletedTask);
        _invitationRepository.Setup(e => e.GetAsync(acceptInvitation.ID)).ReturnsAsync(invitation);

        var result = await _acceptInvitationUseCase.HandleAsync(acceptInvitation);

        _invitationRepository.Verify(e => e.DeleteAsync(acceptInvitation.ID), Times.Once);
    }

    [Test]
    public async Task AcceptInvitation_ValidInvitation_SavesChangesAsyncAtLeastOnce()
    {
        AcceptInvitation acceptInvitation = new()
        {
            ID = 1,
        };

        Invitation invitation = new()
        {
            ID = acceptInvitation.ID,
            UserID = 1,
            BoardID = 1,
        };

        _invitationRepository.Setup(e => e.DeleteAsync(acceptInvitation.ID)).Returns(Task.CompletedTask);
        _invitationRepository.Setup(e => e.GetAsync(acceptInvitation.ID)).ReturnsAsync(invitation);
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, invitation.UserID.ToString()) }));
        context.User = user;
        _contextAccessor.Setup(e => e.HttpContext).Returns(context);

        var result = await _acceptInvitationUseCase.HandleAsync(acceptInvitation);

        _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.AtLeastOnce);
    }

    [Test]
    public async Task AcceptInvitation_NonExistingInvitation_ReturnsErrorResult()
    {
        AcceptInvitation acceptInvitation = new()
        {
            ID = 2,
        };

        _invitationRepository.Setup(e => e.DeleteAsync(acceptInvitation.ID)).Returns(Task.CompletedTask);
        _invitationRepository.Setup(e => e.GetAsync(acceptInvitation.ID)).ReturnsAsync(new Invitation());
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, "1") }));
        context.User = user;
        _contextAccessor.Setup(e => e.HttpContext).Returns(context);

        var result = await _acceptInvitationUseCase.HandleAsync(acceptInvitation);

        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result.Errors!);
        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task AcceptInvitation_NoAuthorizationToAcceptInvitation_ReturnsErrorResult()
    {
        AcceptInvitation acceptInvitation = new()
        {
            ID = 1,
        };

        Invitation invitation = new()
        {
            ID = acceptInvitation.ID,
            UserID = 1,
            BoardID = 1,
        };

        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims: new List<Claim> { new Claim(KanClaimName.ID, "2") }));
        context.User = user;
        _contextAccessor.Setup(e => e.HttpContext).Returns(context);

        _invitationRepository.Setup(e => e.GetAsync(acceptInvitation.ID)).ReturnsAsync(invitation);

        var result = await _acceptInvitationUseCase.HandleAsync(acceptInvitation);

        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result.Errors!);
        Assert.IsFalse(result.IsSuccess);
    }
}
