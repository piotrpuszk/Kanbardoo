using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Domain.Validators;
public class NewBoardValidator : AbstractValidator<NewBoard>
{
	public NewBoardValidator()
	{
		RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
    }

    public override ValidationResult Validate(ValidationContext<NewBoard> context)
    {
		if (context.InstanceToValidate is null)
		{
			return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("newBoard", "The new board is null") });
		}

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<NewBoard> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("newBoard", "The new board is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
