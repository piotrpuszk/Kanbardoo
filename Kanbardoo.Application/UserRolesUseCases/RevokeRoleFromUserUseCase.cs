using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserRolesContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Serilog;

namespace Kanbardoo.Application.UserRolesUseCases;

public class RevokeRoleFromUserUseCase : IRevokeRoleFromUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    //private readonly RevokeRoleFromUserValidator _revokeRoleFromUserValidator;

    //public RevokeRoleFromUserUseCase(IUnitOfWork unitOfWork,
    //                             ILogger logger,
    //                             RevokeRoleFromUserValidator revokeRoleFromUserValidator)
    //{
    //    _unitOfWork = unitOfWork;
    //    _logger = logger;
    //    _revokeRoleFromUserValidator = revokeRoleFromUserValidator;
    //}

    public async Task<Result> HandleAsync(UserRoleRevokeModel userRoleRevokeModel)
    {
        //var validationResult = await _revokeRoleFromUserValidator.ValidateAsync(userRoleRevokeModel);

        //if (!validationResult.IsValid)
        //{
        //    _logger.Error(ErrorMessage.UserRoleInvalid);
        //    return Result.ErrorResult(ErrorMessage.UserRoleInvalid);
        //}

        var user = await _unitOfWork.UserRepository.GetAsync(userRoleRevokeModel.UserName);
        var role = await _unitOfWork.RoleRepository.GetAsync(userRoleRevokeModel.RoleName);
        KanUserBoardRole userRole = new() { UserID = user.ID, RoleID = role.ID };

        try
        {
            await _unitOfWork.UserBoardRolesRepository.DeleteAsync(userRole);
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