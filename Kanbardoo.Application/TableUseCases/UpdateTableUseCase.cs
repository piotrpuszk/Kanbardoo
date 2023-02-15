using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;

public class UpdateTableUseCase : IUpdateTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> HandleAsync(Table table)
    {

        if (table is null || !table.IsValid())
        {
            _logger.Error($"Invalid table to update: {JsonConvert.SerializeObject(table is not null ? table : "null")}");
            return Result.ErrorResult("The table is invalid");
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
            return Result.ErrorResult("Internal server error");
        }
        
    }
}
