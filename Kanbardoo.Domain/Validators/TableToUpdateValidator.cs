using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class TableToUpdateValidator : AbstractValidator<KanTable>
{
	public TableToUpdateValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.BoardID).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.BoardRepository.GetAsync(id);

			return found.Exists();
		});
		RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
		RuleFor(e => e.ID).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.TableRepository.GetAsync(id);

			return found.Exists();
		});
		RuleFor(e => e.CreationDate).Must(e => e != default);
	}

    public override ValidationResult Validate(ValidationContext<KanTable> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("table", "The table is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<KanTable> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("table", "The table is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
