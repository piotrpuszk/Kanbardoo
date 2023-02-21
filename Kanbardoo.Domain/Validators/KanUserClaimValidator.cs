using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class KanUserClaimValidator : AbstractValidator<KanUserClaim>
{
	public KanUserClaimValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e).MustAsync(async (userClaim, token) => 
		{
            var found = await unitOfWork.UserClaimsRepository.GetAsync(userClaim);

			return found.Exists();
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
