using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kanbardoo.Domain.Validators;

public class AcceptInvitationValidator : AbstractValidator<AcceptInvitation>
{
    public AcceptInvitationValidator(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        RuleFor(e => e.ID).MustAsync(async (id, token) =>
        {
            var invitation = await unitOfWork.InvitationRepository.GetAsync(id);

            return invitation.Exists();
        });

        RuleFor(e => e.ID).MustAsync(async (id, token) =>
        {
            var invitation = await unitOfWork.InvitationRepository.GetAsync(id);

            var userID = int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);

            return userID == invitation.UserID;
        });
    }

    public override ValidationResult Validate(ValidationContext<AcceptInvitation> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("acceptInvitation", "The accept invitation is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<AcceptInvitation> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("acceptInvitation", "The accept invitation is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}

