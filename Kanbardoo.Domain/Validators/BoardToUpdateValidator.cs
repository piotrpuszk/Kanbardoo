using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class BoardToUpdateValidator : AbstractValidator<KanBoard>
{
	public BoardToUpdateValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.OwnerID).Must(e => e != default);
		RuleFor(e => e.StatusID).Must(e => e != default);
		RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
		RuleFor(e => e.CreationDate).Must(e => e != default);
		RuleFor(e => e.ID).MustAsync(async (id, token) => 
		{
            var found = await unitOfWork.BoardRepository.GetAsync(id);
			return found.ID != default;
        });
	}

    public override ValidationResult Validate(ValidationContext<KanBoard> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("board", "The board is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<KanBoard> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("board", "The board is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
