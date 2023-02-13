using Kanbardoo.Application.Contracts.TableContracts;
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

    public async Task HandleAsync(int id)
    {
        await _unitOfWork.TableRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
