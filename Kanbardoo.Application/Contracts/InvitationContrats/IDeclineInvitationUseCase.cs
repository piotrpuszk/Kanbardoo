using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.InvitationContrats;

public interface IDeclineInvitationUseCase
{
    Task<Result> HandleAsync(DeclineInvitation declineInvitation);
}