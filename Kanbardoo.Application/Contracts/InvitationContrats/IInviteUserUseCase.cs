using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.InvitationContrats;
public interface IInviteUserUseCase
{
    Task<Result> HandleAsync(NewInvitation newInvitation);
}
