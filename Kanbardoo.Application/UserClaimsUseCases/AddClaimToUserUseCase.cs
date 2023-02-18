using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Serilog;

namespace Kanbardoo.Application.UserClaimsUseCases;
public class AddClaimToUserUseCase : IAddClaimToUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly AddClaimToUserValidator _addClaimToUserValidator;

    public AddClaimToUserUseCase(IUnitOfWork unitOfWork,
                                 ILogger logger,
                                 AddClaimToUserValidator addClaimToUserValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _addClaimToUserValidator = addClaimToUserValidator;
    }

    public async Task<Result> HandleAsync(KanUserClaim userClaim)
    {
        var validationResult = await _addClaimToUserValidator.ValidateAsync(userClaim);

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
