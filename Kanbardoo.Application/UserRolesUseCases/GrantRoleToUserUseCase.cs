using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserRolesContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Serilog;

namespace Kanbardoo.Application.UserRolesUseCases;

public class GrantRoleToUserUseCase : IGrantRoleToUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly GrantBoardRoleToUserValidator _grantBoardRoleToUserValidator;

    public GrantRoleToUserUseCase(IUnitOfWork unitOfWork,
                                 ILogger logger,
                                 GrantBoardRoleToUserValidator grantBoardRoleToUserValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _grantBoardRoleToUserValidator = grantBoardRoleToUserValidator;
    }

    public async Task<Result> HandleAsync(UserBoardRoleGrantModel userRoleGrantModel)
    {
        var validationResult = await _grantBoardRoleToUserValidator.ValidateAsync(userRoleGrantModel);

        if (!validationResult.IsValid)
        {
            _logger.Error(ErrorMessage.UserRoleInvalid);
            return Result.ErrorResult(ErrorMessage.UserRoleInvalid);
        }

        var user = await _unitOfWork.UserRepository.GetAsync(userRoleGrantModel.UserName);
        var role = await _unitOfWork.RoleRepository.GetAsync(userRoleGrantModel.RoleName);
        KanUserBoardRole userRole = new() { UserID = user.ID, RoleID = role.ID, BoardID = userRoleGrantModel.BoardID };

        try
        {
            await _unitOfWork.UserBoardRolesRepository.AddAsync(userRole);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"{ErrorMessage.InternalServerError}: \n\n {userRole} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError);
        }
    }
}
