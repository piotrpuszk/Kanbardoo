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

    public GetTableUseCase(ILogger logger,
                           IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<Table>>> HandleAsync()
    {
        try
        {
            var tables = await _unitOfWork.TableRepository.GetAsync();
            return Result<IEnumerable<Table>>.SuccessResult(tables);
        }
        catch(Exception ex)
        {
            _logger.Error($"{nameof(GetTableUseCase)}.{nameof(HandleAsync)} \n\n {ex}");
            return Result<IEnumerable<Table>>.ErrorResult("Internal server error", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<Table>> HandleAsync(int id)
    {
        Table table = new Table();
        try
        {
            table = await _unitOfWork.TableRepository.GetAsync(id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error {id} \n\n {ex}");
            return ErrorResult<Table>.ErrorResult($"Internal server error", HttpStatusCode.InternalServerError);
        }

        if (!table.Exists())
        {
            _logger.Error($"A table with the give id {id} does not exist");
            return ErrorResult<Table>.ErrorResult($"A table with the give id {id} does not exist");
        }

        return Result<Table>.SuccessResult(table);
    }
}