using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Serilog;

namespace Kanbardoo.Application.UserContracts;

public class GetUsersUseCase : IGetUsersUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public GetUsersUseCase(IUnitOfWork unitOfWork,
                           ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<KanUser>>> HandleAsync(string query)
    {
        var result = await _unitOfWork.UserRepository.GetUsersAsync(query);

        return Result<IEnumerable<KanUser>>.SuccessResult(result);
    }
}