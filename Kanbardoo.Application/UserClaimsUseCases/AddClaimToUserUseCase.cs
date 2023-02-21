using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Serilog;

namespace Kanbardoo.Application.UserClaimsUseCases;
public class AddClaimToUserUseCase : IAddClaimToUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly NewKanUserClaimValidator _newClaimToUserValidator;

    public AddClaimToUserUseCase(IUnitOfWork unitOfWork,
                                 ILogger logger,
                                 NewKanUserClaimValidator newClaimToUserValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _newClaimToUserValidator = newClaimToUserValidator;
    }

    public async Task<Result> HandleAsync(KanUserClaimModel userClaimModel)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(userClaimModel.UserName);
        var claim = await _unitOfWork.ClaimRepository.GetAsync(userClaimModel.ClaimName);
        KanUserClaim userClaim = new() { UserID = user.ID, ClaimID = claim.ID };
        var validationResult = await _newClaimToUserValidator.ValidateAsync(userClaim);

        if (!validationResult.IsValid)
        {
            _logger.Error(ErrorMessage.UserClaimInvalid);
            return Result.ErrorResult(ErrorMessage.UserClaimInvalid);
        }

        try
        {
            await _unitOfWork.UserClaimsRepository.AddAsync(userClaim);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch(Exception ex)
        {
            _logger.Error($"{ErrorMessage.InternalServerError}: \n\n {userClaim} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError);
        }
    }
}
