using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class NewTaskValidator : AbstractValidator<NewKanTask>
{
	public NewTaskValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.AssigneeID).MustAsync(async (id, token) => 
		{
			var found = await unitOfWork.UserRepository.GetAsync(id);

			return found.ID != default;
		});
		RuleFor(e => e.StatusID).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.TaskStatusRepository.GetAsync(id);

			return found.ID != default;
		});
		RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
		RuleFor(e => e.TableID).MustAsync(async (id, token) =>
		{
			var found = await unitOfWork.TableRepository.GetAsync(id);

			return found.ID != default;
		});
	}

    public override ValidationResult Validate(ValidationContext<NewKanTask> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("newTask", "The new task is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<NewKanTask> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("newTask", "The new task is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
