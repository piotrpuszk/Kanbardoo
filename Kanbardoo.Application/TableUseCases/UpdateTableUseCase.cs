﻿using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;

public class UpdateTableUseCase : IUpdateTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TableToUpdateValidator _tableToUpdateValidator;
    private readonly ITableMembershipPolicy _tableMembershipPolicy;

    public UpdateTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           TableToUpdateValidator tableToUpdateValidator,
                           ITableMembershipPolicy tableMembershipPolicy)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _tableToUpdateValidator = tableToUpdateValidator;
        _tableMembershipPolicy = tableMembershipPolicy;
    }
    public async Task<Result> HandleAsync(KanTable table)
    {
        var validationResult = await _tableToUpdateValidator.ValidateAsync(table);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Invalid table to update: {JsonConvert.SerializeObject(table is not null ? table : "null")}");
            return Result.ErrorResult(ErrorMessage.GivenTableInvalid);
        }

        var authorizationResult = await _tableMembershipPolicy.AuthorizeAsync(table.ID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        try
        {
            await _unitOfWork.TableRepository.UpdateAsync(table);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch(Exception ex)
        {
            _logger.Error($"Internal server error {JsonConvert.SerializeObject(table)} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
        
    }
}
