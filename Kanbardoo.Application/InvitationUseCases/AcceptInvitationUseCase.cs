using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace Kanbardoo.Application.InvitationUseCases;

public class AcceptInvitationUseCase : IAcceptInvitationUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly AcceptInvitationValidator _newInvitationValidator;

    public AcceptInvitationUseCase(IUnitOfWork unitOfWork,
                             ILogger logger,
                             AcceptInvitationValidator newInvitationValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newInvitationValidator = newInvitationValidator;
    }

    public async Task<Result> HandleAsync(AcceptInvitation acceptInvitation)
    {
        var validationResult = await _newInvitationValidator.ValidateAsync(acceptInvitation);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Invalid invitation:\r\n\r\n {JsonConvert.SerializeObject(acceptInvitation ?? new AcceptInvitation())}");
            return Result.ErrorResult(ErrorMessage.InvitationInvalid);
        }

        var invitation = await _unitOfWork.InvitationRepository.GetAsync(acceptInvitation.ID);
        await _unitOfWork.InvitationRepository.DeleteAsync(acceptInvitation.ID);
        KanUserBoardRole kanUserBoardRole = new()
        {
            UserID = invitation.UserID,
            BoardID = invitation.BoardID,
            RoleID = KanRoleID.Member,
            User = null!,
            Board = null!,
            Role = null!,
        };
        await _unitOfWork.UserBoardRolesRepository.AddAsync(kanUserBoardRole);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.Error($"{ErrorMessage.InternalServerError} \r\n\r\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }

        return Result.SuccessResult();
    }
}