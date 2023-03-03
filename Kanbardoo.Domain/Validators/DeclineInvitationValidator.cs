using FluentValidation;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kanbardoo.Domain.Validators;
public class DeclineInvitationValidator : AbstractValidator<DeclineInvitation>
{
	public DeclineInvitationValidator(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
	{
		RuleFor(e => e.InvitationID).Must(e => e != default);
		RuleFor(e => e.InvitationID).MustAsync(async (id, token) => 
		{
			var userID = int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);

			var invitation = await unitOfWork.InvitationRepository.GetAsync(id);

			return invitation.Exists() && invitation.UserID == userID;
		});
	}
}
