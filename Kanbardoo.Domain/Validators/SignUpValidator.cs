﻿using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Domain.Validators;
public class SignUpValidator : AbstractValidator<SignUp>
{
	public SignUpValidator(IUnitOfWork unitOfWork)
	{
		RuleFor(e => e.UserName).Must(e => !string.IsNullOrWhiteSpace(e));
		RuleFor(e => e.Password).Must(e => !string.IsNullOrWhiteSpace(e));
        RuleFor(e => e.UserName).MustAsync(async (username, token) => 
        {
            var foundUser = await unitOfWork.UserRepository.GetAsync(username);

            return !foundUser.Exists();
        });
	}

    public override ValidationResult Validate(ValidationContext<SignUp> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("signUp", "The sign up data are null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<SignUp> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("signUp", "The sign up data are null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}
