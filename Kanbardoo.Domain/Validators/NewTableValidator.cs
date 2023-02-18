using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class NewTableValidator : AbstractValidator<NewKanTable>
{
	public NewTableValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.BoardID).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.BoardRepository.GetAsync(id);

			return found.ID != default;
		});

		RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
		RuleFor(e => e.Priority).Must(e => e >= 0);
	}

    public override ValidationResult Validate(ValidationContext<NewKanTable> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("newTable", "The new table is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<NewKanTable> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("newTable", "The new table is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
