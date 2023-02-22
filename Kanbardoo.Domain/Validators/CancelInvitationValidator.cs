using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Domain.Validators;

public class CancelInvitationValidator : AbstractValidator<CancelInvitationModel>
{ 
    public CancelInvitationValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(e => e.UserName).MustAsync(async (username, token) =>
        {
            var found = await unitOfWork.UserRepository.GetAsync(username);
            return found.Exists();
        });
        RuleFor(e => e.BoardID).MustAsync(async (id, token) =>
        {
            var found = await unitOfWork.BoardRepository.GetAsync(id);
            return found.Exists();
        });
    }

    public override ValidationResult Validate(ValidationContext<CancelInvitationModel> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("board", "The cancel invitation request is null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<CancelInvitationModel> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("board", "The cancel invitation request is null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}