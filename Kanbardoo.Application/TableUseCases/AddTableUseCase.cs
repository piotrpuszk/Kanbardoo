using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
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

    public async Task<Result> HandleAsync(NewTable newTable)
    {
        if (newTable is null)
        {
            _logger.Error("tried to add a newTable = null");
            return Result.ErrorResult("The given table is null");
        }

        Board board = await _unitOfWork.BoardRepository.GetAsync(newTable.BoardID);

        if (!board.Exists())
        {
            _logger.Error($"A new table tried to add to a non-existing board: {JsonConvert.SerializeObject(newTable)}");
            return Result.ErrorResult("A new table tried to add to a non-existing board");
        }

        Table table = new()
        {
            BoardID= newTable.BoardID,
            Name= newTable.Name,
            CreationDate = DateTime.UtcNow,
            Priority= newTable.Priority,
        };

        try
        {
            await _unitOfWork.TableRepository.AddAsync(table);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }
        catch(Exception ex)
        {
            _logger.Error($"Internal server error: \n\n {nameof(AddTableUseCase)}.{nameof(HandleAsync)}({JsonConvert.SerializeObject(newTable)})");
            return Result.ErrorResult("Internal server error");
        }
        
    }
}
