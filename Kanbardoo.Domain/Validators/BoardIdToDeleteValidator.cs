using FluentValidation;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;
public class BoardIdToDeleteValidator : AbstractValidator<int>
{
	public BoardIdToDeleteValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(id => id).MustAsync(async (id, token) => 
		{
			var foundBoard = await unitOfWork.BoardRepository.GetAsync(id);

			return foundBoard.Exists();
        });
	}
}
