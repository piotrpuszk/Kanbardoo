using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class AddClaimToUserValidator : AbstractValidator<KanUserClaim>
{
	public AddClaimToUserValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.UserID).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.UserRepository.GetAsync(id);

			return found.Exists();
		});

		RuleFor(e => e.ClaimID).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.ClaimRepository.GetAsync(id);

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
