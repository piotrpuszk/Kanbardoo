using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Domain.Validators;
public class BoardFiltersValidator : AbstractValidator<BoardFilters>
{
	public BoardFiltersValidator()
	{
		RuleFor(e => e.OrderByClauses).ForEach(e => e.Must(e => Entity.ColumnExists<Board>(e.ColumnName)));
	}

    public override ValidationResult Validate(ValidationContext<BoardFilters> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("boardFilters", "The board filters are null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<BoardFilters> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("boardFilters", "The board filters are null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
