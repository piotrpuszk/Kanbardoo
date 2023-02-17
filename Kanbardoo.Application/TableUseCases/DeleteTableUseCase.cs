using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;

public class DeleteTableUseCase : IDeleteTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TableIDToDelete _tableIDToDelete;

    public DeleteTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           TableIDToDelete tableIDToDelete)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _tableIDToDelete = tableIDToDelete;
    }

    public async Task<Result> HandleAsync(int id)
    {
        var validationResult = await _tableIDToDelete.ValidateAsync(id);
        if (!validationResult.IsValid)
        {
            _logger.Error($"The table id is invalid {id}");
            return Result.ErrorResult("The table id is invalid");
        }

        try
        {
            await _unitOfWork.TableRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error: {id} \n\n {ex}");
            return Result.ErrorResult("Internal server error");
        }
        
    }
}
