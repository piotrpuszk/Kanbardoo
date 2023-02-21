using Kanbardoo.Domain.Entities;

namespace Kanbardoo.Application.Contracts.InvitationContrats;

public interface IGetInvitationsUseCase
{
    Task<Result<IEnumerable<Invitation>>> HandleAsync();
}