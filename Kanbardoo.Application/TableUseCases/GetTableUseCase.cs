using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;
public class GetTableUseCase : IGetTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Table>> HandleAsync()
    {
        return await _unitOfWork.TableRepository.GetAsync();
    }

    public async Task<Table> HandleAsync(int id)
    {
        return await _unitOfWork.TableRepository.GetAsync(id);
    }
}