using FluentValidation;
using FluentValidation.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kanbardoo.Domain.Validators;

public class GrantBoardRoleToUserValidator : AbstractValidator<UserBoardRoleGrantModel>
{
    public GrantBoardRoleToUserValidator(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        RuleFor(e => e).MustAsync(async (userRoleGrantModel, token) =>
        {
            var user = await unitOfWork.UserRepository.GetAsync(userRoleGrantModel.UserName);
            var role = await unitOfWork.RoleRepository.GetAsync(userRoleGrantModel.RoleName);

            if (!user.Exists() || !role.Exists())
            {
                return false;
            }

            KanUserBoardRole userBoardRole = new()
            {
                UserID = int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!),
                RoleID = role.ID,
            };

            var result = await unitOfWork.UserBoardRolesRepository.GetAsync(userBoardRole);

            return result.Exists() && result.RoleID == KanRoleID.Owner;
        });
    }

    public override ValidationResult Validate(ValidationContext<UserBoardRoleGrantModel> context)
    {
        if (context.InstanceToValidate is null)
        {
            return new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userBoardRoleGrantModel", "The user role are null") });
        }

        return base.Validate(context);
    }

    public override Task<ValidationResult> ValidateAsync(ValidationContext<UserBoardRoleGrantModel> context, CancellationToken cancellation = default)
    {
        if (context.InstanceToValidate is null)
        {
            return Task.FromResult(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("userBoardRoleGrantModel", "The user role are null") }));
        }

        return base.ValidateAsync(context, cancellation);
    }
}