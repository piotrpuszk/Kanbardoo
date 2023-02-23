using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;

namespace Kanbardoo.Application.InvitationUseCases;

public class GetInvitationsUseCase : IGetInvitationsUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly int _userID;

    public GetInvitationsUseCase(IUnitOfWork unitOfWork,
                             ILogger logger,
                             IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result<IEnumerable<Invitation>>> HandleAsync()
    {
        try
        {
            var invitations = await _unitOfWork.InvitationRepository.GetUserInvitationsAsync(_userID);
            return Result<IEnumerable<Invitation>>.SuccessResult(invitations);
        }
        catch (Exception ex)
        {
            _logger.Error($"{ErrorMessage.InternalServerError} \r\n\r\n {ex}");
            return Result<IEnumerable<Invitation>>.ErrorResult(ErrorMessage.InternalServerError);
        }
    }
}
