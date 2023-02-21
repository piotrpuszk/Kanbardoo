using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;

public class NewKanUserClaimValidator : AbstractValidator<KanUserClaim>
{
    public NewKanUserClaimValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(e => e.UserID).MustAsync(async (userID, token) =>
        {
            var user = await unitOfWork.UserRepository.GetAsync(userID);

            return user.Exists();
        });
        RuleFor(e => e.ClaimID).MustAsync(async (claimID, token) =>
        {
            var claim = await unitOfWork.ClaimRepository.GetAsync(claimID);

            return claim.Exists();
        });
    }

    public override ValidationResult Validate(ValidationContext<KanUserClaim> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userClaims", "The user claims are null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<KanUserClaim> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userClaims", "The user claims are null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}