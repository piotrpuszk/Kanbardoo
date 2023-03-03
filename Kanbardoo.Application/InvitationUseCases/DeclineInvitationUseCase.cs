using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using Serilog;

namespace Kanbardoo.Application.InvitationUseCases;

public class DeclineInvitationUseCase : IDeclineInvitationUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly DeclineInvitationValidator _declineInvitationValidator;

    public DeclineInvitationUseCase(IUnitOfWork unitOfWork,
                             ILogger logger,
                             DeclineInvitationValidator declineInvitationValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _declineInvitationValidator = declineInvitationValidator;
    }

    public async Task<Result> HandleAsync(DeclineInvitation declineInvitation)
    {
        var validationResult = await _declineInvitationValidator.ValidateAsync(declineInvitation);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Invalid invitation:\r\n\r\n {JsonConvert.SerializeObject(declineInvitation ?? new DeclineInvitation())}");
            return Result.ErrorResult(ErrorMessage.InvitationInvalid);
        }

        await _unitOfWork.InvitationRepository.DeleteAsync(declineInvitation.InvitationID);

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