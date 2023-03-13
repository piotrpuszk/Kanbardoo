using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Security.Claims;

namespace Kanbardoo.Application.InvitationUseCases;
public class InviteUserUseCase : IInviteUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly NewInvitationValidator _newInvitationValidator;
    private readonly IBoardOwnershipPolicy _boardOwnershipPolicy;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InviteUserUseCase(IUnitOfWork unitOfWork,
                             ILogger logger,
                             NewInvitationValidator newInvitationValidator,
                             IBoardOwnershipPolicy boardOwnershipPolicy,
                             IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newInvitationValidator = newInvitationValidator;
        _boardOwnershipPolicy = boardOwnershipPolicy;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result> HandleAsync(NewInvitation newInvitation)
    {
        var validationResult = await _newInvitationValidator.ValidateAsync(newInvitation);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Invalid invitation:\r\n\r\n {JsonConvert.SerializeObject(newInvitation ?? new NewInvitation())}");
            return Result.ErrorResult(ErrorMessage.InvitationInvalid);
        }

        var authorizationResult = await _boardOwnershipPolicy.AuthorizeAsync(newInvitation.BoardID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        var user = await _unitOfWork.UserRepository.GetAsync(newInvitation.UserName);
        var senderID = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
        Invitation invitation = new()
        {
            UserID = user.ID,
            BoardID = newInvitation.BoardID,
            SenderID = senderID,
        };

        await _unitOfWork.InvitationRepository.AddAsync(invitation);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error($"{ErrorMessage.InternalServerError} \r\n\r\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError);
        }

        return Result.SuccessResult();
    }
}
