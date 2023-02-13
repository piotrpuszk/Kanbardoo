using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;
public class AddTableUseCase : IAddTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(NewTable newTable)
    {
        Table table = new()
        {
            BoardID= newTable.BoardID,
            Name= newTable.Name,
            CreationDate = DateTime.UtcNow,
            Priority= newTable.Priority,
        };

        await _unitOfWork.TableRepository.AddAsync(table);
        await _unitOfWork.SaveChangesAsync();
    }
}
