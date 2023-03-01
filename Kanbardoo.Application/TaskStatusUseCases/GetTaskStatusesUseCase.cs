using Kanbardoo.Application.Contracts.TaskStatusContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Serilog;

namespace Kanbardoo.Application.TaskStatusUseCases;
public class GetTaskStatusesUseCase : IGetTaskStatusesUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public GetTaskStatusesUseCase(IUnitOfWork unitOfWork,
                                  ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<KanTaskStatus>>> HandleAsync()
    {
        var result = await _unitOfWork.TaskStatusRepository.GetAsync();

        return Result<IEnumerable<KanTaskStatus>>.SuccessResult(result);
    }
}
