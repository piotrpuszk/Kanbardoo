﻿using Kanbardoo.Application.Authorization.PolicyContracts;
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

public class CancelInvitationUseCase : ICancelInvitationUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly CancelInvitationValidator _cancelInvitationValidator;
    private readonly IBoardOwnershipPolicy _boardOwnershipPolicy;

    public CancelInvitationUseCase(IUnitOfWork unitOfWork,
                             ILogger logger,
                             CancelInvitationValidator cancelInvitationValidator,
                             IBoardOwnershipPolicy boardOwnershipPolicy)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cancelInvitationValidator = cancelInvitationValidator;
        _boardOwnershipPolicy = boardOwnershipPolicy;
    }

    public async Task<Result> HandleAsync(CancelInvitationModel cancelInvitationModel)
    {
        var validationResult = await _cancelInvitationValidator.ValidateAsync(cancelInvitationModel);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Invalid invitation:\r\n\r\n {JsonConvert.SerializeObject(cancelInvitationModel ?? new CancelInvitationModel())}");
            return Result.ErrorResult(ErrorMessage.InvitationInvalid);
        }

        var authorizationResult = await _boardOwnershipPolicy.AuthorizeAsync(cancelInvitationModel.BoardID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        var user = await _unitOfWork.UserRepository.GetAsync(cancelInvitationModel.UserName);
        Invitation invitation = new()
        {
            UserID = user.ID,
            BoardID = cancelInvitationModel.BoardID,
        };

        await _unitOfWork.InvitationRepository.DeleteAsync(invitation);

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
