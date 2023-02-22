using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using Serilog;

namespace Kanbardoo.Application.InvitationUseCases;
public class InviteUserUseCase : IInviteUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly NewInvitationValidator _newInvitationValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;

    public InviteUserUseCase(IUnitOfWork unitOfWork,
                             ILogger logger,
                             NewInvitationValidator newInvitationValidator,
                             IBoardMembershipPolicy boardMembershipPolicy)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newInvitationValidator = newInvitationValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
    }

    public async Task<Result> HandleAsync(NewInvitation newInvitation)
    {
        var validationResult = await _newInvitationValidator.ValidateAsync(newInvitation);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Invalid invitation:\r\n\r\n {JsonConvert.SerializeObject(newInvitation ?? new NewInvitation())}");
            return Result.ErrorResult(ErrorMessage.InvitationInvalid);
        }

        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(newInvitation.BoardID);
        if (!authorizationResult.IsSuccess)
        {
            return Result.ErrorResult(ErrorMessage.Unauthorized);
        }

        var user = await _unitOfWork.UserRepository.GetAsync(newInvitation.UserName);
        Invitation invitation = new()
        {
            UserID = user.ID,
            BoardID = newInvitation.BoardID,
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
