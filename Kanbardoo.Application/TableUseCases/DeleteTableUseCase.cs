using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;

public class DeleteTableUseCase : IDeleteTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(int id)
    {
        Table table = await _unitOfWork.TableRepository.GetAsync(id);

        if (!table.Exists())
        {
            _logger.Error($"A table with the given id ({id}) does not exist");
            return Result.ErrorResult("A table with the given id does not exist");
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
