using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Domain.Validators;
public class KanTaskValidator : AbstractValidator<KanTask>
{
	public KanTaskValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
        RuleFor(e => e.StatusID).MustAsync(async (id, token) => 
		{
			var status = await unitOfWork.TaskStatusRepository.GetAsync(id);

			return status.Exists();
		});
	}

    public override ValidationResult Validate(ValidationContext<KanTask> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("task", "The task is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<KanTask> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("task", "The task is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
