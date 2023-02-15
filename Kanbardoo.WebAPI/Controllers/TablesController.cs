using AutoMapper;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TablesController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IAddTableUseCase _addTableUseCase;
    private readonly IUpdateTableUseCase _updateTableUseCase;
    private readonly IDeleteTableUseCase _deleteTableUseCase;
    private readonly IGetTableUseCase _getTableUseCase;

    public TablesController(ILogger logger,
                            IMapper mapper,
                            IAddTableUseCase addTableUseCase,
                            IUpdateTableUseCase updateTableUseCase,
                            IDeleteTableUseCase deleteTableUseCase,
                            IGetTableUseCase getTableUseCase)
    {
        _logger = logger;
        _mapper = mapper;
        _addTableUseCase = addTableUseCase;
        _updateTableUseCase = updateTableUseCase;
        _deleteTableUseCase = deleteTableUseCase;
        _getTableUseCase = getTableUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _getTableUseCase.HandleAsync();

        if (!result.IsSuccess)
        {
            return Ok(Result<IEnumerable<TableDTO>>.ErrorResult(result.Errors!));
        }

        var tableDTOs = _mapper.Map<IEnumerable<TableDTO>>(result.Content);
        return Ok(Result<IEnumerable<TableDTO>>.SuccessResult(tableDTOs));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _getTableUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return Ok(Result<TableDTO>.ErrorResult(result.Errors!));
        }

        var tableDTO = _mapper.Map<TableDTO>(result.Content);
        return Ok(Result<TableDTO>.SuccessResult(tableDTO));
    }

    [HttpPost]
    public async Task<IActionResult> Post(NewTableDTO newTableDTO)
    {
        var newTable = _mapper.Map<NewTable>(newTableDTO);
        var result = await _addTableUseCase.HandleAsync(newTable);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(TableDTO tableDTO)
    {
        var table = _mapper.Map<Table>(tableDTO);
        var result = await _updateTableUseCase.HandleAsync(table);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteTableUseCase.HandleAsync(id);
        return Ok(result);
    }

}
