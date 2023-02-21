using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TableUseCases;
public class GetTableUseCase : IGetTableUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITableMembershipPolicy _tableMembershipPolicy;

    public GetTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           ITableMembershipPolicy tableMembershipPolicy)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _tableMembershipPolicy = tableMembershipPolicy;
    }

    public async Task<Result<IEnumerable<KanTable>>> HandleAsync()
    {
        try
        {
            var tables = await _unitOfWork.TableRepository.GetAsync();
            return Result<IEnumerable<KanTable>>.SuccessResult(tables);
        }
        catch(Exception ex)
        {
            _logger.Error($"{nameof(GetTableUseCase)}.{nameof(HandleAsync)} \n\n {ex}");
            return Result<IEnumerable<KanTable>>.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<KanTable>> HandleAsync(int id)
    {
        KanTable table;
        try
        {
            table = await _unitOfWork.TableRepository.GetAsync(id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error {id} \n\n {ex}");
            return ErrorResult<KanTable>.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }

        if (!table.Exists())
        {
            _logger.Error($"A table with the give id {id} does not exist");
            return ErrorResult<KanTable>.ErrorResult(ErrorMessage.TableWithIDNotExist);
        }

        var authorizationResult = await _tableMembershipPolicy.Authorize(table.ID);
        if (!authorizationResult.IsSuccess)
        {
            return Result<KanTable>.ErrorResult(authorizationResult.Errors!);
        }

        return Result<KanTable>.SuccessResult(table);
    }
}