using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.InvitationContrats;

public interface IAcceptInvitationUseCase
{
    Task<Result> HandleAsync(AcceptInvitation acceptInvitation);
}
