using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;

namespace Kanbardoo.Domain.Validators;
public class BoardFiltersValidator : AbstractValidator<KanBoardFilters>
{
	public BoardFiltersValidator()
	{
		RuleFor(e => e.OrderByClauses).ForEach(e => e.Must(e => Entity.ColumnExists<KanBoard>(e.ColumnName)));
	}

    public override ValidationResult Validate(ValidationContext<KanBoardFilters> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("boardFilters", "The board filters are null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<KanBoardFilters> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("boardFilters", "The board filters are null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
