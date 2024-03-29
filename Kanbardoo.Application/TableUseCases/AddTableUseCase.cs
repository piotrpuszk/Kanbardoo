﻿using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Nodes;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;
public class AddTableUseCase : IAddTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NewTableValidator _newTableValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;
    private readonly int _userID;

    public AddTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           NewTableValidator newTableValidator,
                           IBoardMembershipPolicy boardMembershipPolicy,
                           IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _newTableValidator = newTableValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
        _userID = int.Parse(contextAccessor.HttpContext!.User.FindFirstValue(KanClaimName.ID)!);
    }

    public async Task<Result> HandleAsync(NewKanTable newTable)
    {
        var validationResult = await _newTableValidator.ValidateAsync(newTable);
        if (!validationResult.IsValid)
        {
            _logger.Error(ErrorMessage.GivenTableInvalid);
            return Result.ErrorResult(ErrorMessage.GivenTableInvalid);
        }

        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(newTable.BoardID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        KanTable table = new()
        {
            BoardID= newTable.BoardID,
            Name= newTable.Name,
            CreationDate = DateTime.UtcNow,
            Priority= newTable.Priority,
        };

        try
        {
            await _unitOfWork.TableRepository.AddAsync(table);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }
        catch(Exception ex)
        {
            _logger.Error($"Internal server error: \n\n {nameof(AddTableUseCase)}.{nameof(HandleAsync)}({JsonConvert.SerializeObject(newTable)}) \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
        
    }
}
