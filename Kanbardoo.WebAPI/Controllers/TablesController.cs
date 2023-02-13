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
        IEnumerable<Table> tables = await _getTableUseCase.HandleAsync();
        var tableDTOs = _mapper.Map<IEnumerable<Table>>(tables);
        return Ok(tableDTOs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Table table = await _getTableUseCase.HandleAsync(id);
        var tableDTO = _mapper.Map<TableDTO>(table);
        return Ok(tableDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Post(NewTableDTO newTableDTO)
    {
        var newTable = _mapper.Map<NewTable>(newTableDTO);
        await _addTableUseCase.HandleAsync(newTable);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put(TableDTO tableDTO)
    {
        var table = _mapper.Map<Table>(tableDTO);
        await _updateTableUseCase.HandleAsync(table);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _deleteTableUseCase.HandleAsync(id);
        return Ok();
    }

}
