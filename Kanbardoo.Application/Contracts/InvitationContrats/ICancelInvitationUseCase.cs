using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.InvitationContrats;

public interface ICancelInvitationUseCase
{
    Task<Result> HandleAsync(CancelInvitationModel cancelInvitationModel);
}
