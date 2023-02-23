using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;

public class AcceptInvitationValidator : AbstractValidator<AcceptInvitation>
{
    public AcceptInvitationValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(e => e.ID).MustAsync(async (id, token) =>
        {
            var invitation = await unitOfWork.InvitationRepository.GetAsync(id);

            return invitation.Exists();
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

