using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Repositories;
using ILogger = Serilog.ILogger;  

namespace Kanbardoo.Application.BoardUseCases;
public class DeleteBoardUseCase : IDeleteBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoardUseCase(IUnitOfWork unitOfWork,
                              ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task HandleAsync(int id)
    {
        await _unitOfWork.BoardRepository.DeleteAsync(id);

        await _unitOfWork.SaveChangesAsync();
    }
}
