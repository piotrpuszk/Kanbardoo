using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Serilog;

namespace Kanbardoo.Application.UserClaimsUseCases;

public class RevokeClaimFromUserUseCase : IRevokeClaimFromUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly KanUserClaimValidator _addClaimToUserValidator;

    public RevokeClaimFromUserUseCase(IUnitOfWork unitOfWork,
                                 ILogger logger,
                                 KanUserClaimValidator addClaimToUserValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _addClaimToUserValidator = addClaimToUserValidator;
    }

    public async Task<Result> HandleAsync(KanUserClaimModel userClaimModel)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userClaimModel.UserName);
        var claim = await _unitOfWork.ClaimRepository.GetAsync(userClaimModel.ClaimName);
        KanUserClaim userClaim = new() { UserID = user.ID, ClaimID = claim.ID };
        var validationResult = await _addClaimToUserValidator.ValidateAsync(userClaim);

        if (!validationResult.IsValid)
        {
            _logger.Error(ErrorMessage.UserClaimInvalid);
            return Result.ErrorResult(ErrorMessage.UserClaimInvalid);
        }

        try
        {
            await _unitOfWork.UserClaimsRepository.DeleteAsync(userClaim);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"{ErrorMessage.InternalServerError}: \n\n {userClaim} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError);
        }
    }
}