using AutoMapper;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Authorize]
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
            return Result<IEnumerable<KanTableDTO>>.ErrorResult(result.Errors!, result.HttpCode).GetActionResult();
        }

        var tableDTOs = _mapper.Map<IEnumerable<KanTableDTO>>(result.Content);
        return Result<IEnumerable<KanTableDTO>>.SuccessResult(tableDTOs).GetActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _getTableUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return Result<KanTableDTO>.ErrorResult(result.Errors!, result.HttpCode).GetActionResult();
        }

        var tableDTO = _mapper.Map<KanTableDTO>(result.Content);
        return Result<KanTableDTO>.SuccessResult(tableDTO).GetActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Post(NewKanTableDTO newTableDTO)
    {
        var newTable = _mapper.Map<NewKanTable>(newTableDTO);
        var result = await _addTableUseCase.HandleAsync(newTable);
        return result.GetActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> Put(KanTableDTO tableDTO)
    {
        var table = _mapper.Map<KanTable>(tableDTO);
        var result = await _updateTableUseCase.HandleAsync(table);
        return result.GetActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteTableUseCase.HandleAsync(id);
        return result.GetActionResult();
    }

}
