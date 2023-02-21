using FluentValidation;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class KanTaskIdToDeleteValidator : AbstractValidator<int>
{
	public KanTaskIdToDeleteValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.TaskRepository.GetAsync(id);

			return found.Exists();
		});
	}
}
