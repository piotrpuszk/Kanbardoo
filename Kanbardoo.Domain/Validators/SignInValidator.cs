using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Domain.Validators;

public class SignInValidator : AbstractValidator<SignIn>
{
    public SignInValidator()
    {
        RuleFor(e => e.Name).Must(e => !string.IsNullOrWhiteSpace(e));
        RuleFor(e => e.Password).Must(e => !string.IsNullOrWhiteSpace(e));
    }

    public override ValidationResult Validate(ValidationContext<SignIn> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("signIn", "The sign in data are null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<SignIn> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("signIn", "The sign in data are null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}