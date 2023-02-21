using FluentValidation;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class TableIDToDelete : AbstractValidator<int>
{
	public TableIDToDelete(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e).MustAsync(async (id, token) => 
		{ 
			var found = await unitOfWork.TableRepository.GetAsync(id);

			return found.Exists();
		});
	}
}
