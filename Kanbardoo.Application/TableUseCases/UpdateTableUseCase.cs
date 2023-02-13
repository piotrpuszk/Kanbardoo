using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
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
    public async Task HandleAsync(Table table)
    {
        await _unitOfWork.TableRepository.UpdateAsync(table);
        await _unitOfWork.SaveChangesAsync();
    }
}
