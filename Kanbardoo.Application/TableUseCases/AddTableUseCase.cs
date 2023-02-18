using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Nodes;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;
public class AddTableUseCase : IAddTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NewTableValidator _newTableValidator;

    public AddTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           NewTableValidator newTableValidator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _newTableValidator = newTableValidator;
    }

    public async Task<Result> HandleAsync(NewTable newTable)
    {
        var validationResult = await _newTableValidator.ValidateAsync(newTable);
        if (!validationResult.IsValid)
        {
            _logger.Error($"newTable is invalid");
            return Result.ErrorResult("The given table is invalid");
        }

        Table table = new()
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
            return Result.ErrorResult("Internal server error", HttpStatusCode.InternalServerError);
        }
        
    }
}
