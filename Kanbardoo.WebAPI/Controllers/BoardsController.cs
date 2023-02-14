using AutoMapper;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Kanbardoo.WebAPI.FilterDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public sealed class BoardsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IAddBoardUseCase _addBoardUseCase;
    private readonly IGetBoardUseCase _getBoardUseCase;
    private readonly IUpdateBoardUseCase _updateBoardUseCase;
    private readonly IDeleteBoardUseCase _deleteBoardUseCase;
    private readonly IMapper _mapper;

    public BoardsController(ILogger logger,
                           IAddBoardUseCase addBoardUseCase,
                           IGetBoardUseCase getBoardUseCase,
                           IUpdateBoardUseCase updateBoardUseCase,
                           IMapper mapper,
                           IDeleteBoardUseCase deleteBoardUseCase)
    {
        _logger = logger;
        _addBoardUseCase = addBoardUseCase;
        _getBoardUseCase = getBoardUseCase;
        _updateBoardUseCase = updateBoardUseCase;
        _mapper = mapper;
        _deleteBoardUseCase = deleteBoardUseCase;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Post(NewBoardDTO newBoardDTO)
    {
        NewBoard newBoard = _mapper.Map<NewBoard>(newBoardDTO);
        var result = await _addBoardUseCase.HandleAsync(newBoard);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Get(BoardFiltersDTO boardFiltersDTO)
    {
        var boardFilters = _mapper.Map<BoardFilters>(boardFiltersDTO);
        var result = await _getBoardUseCase.HandleAsync(boardFilters);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var boardDTOs = _mapper.Map<IEnumerable<BoardDTO>>(result.Content);
        return Ok(Result<IEnumerable<BoardDTO>>.SuccessResult(boardDTOs));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _getBoardUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        var boardDTO = _mapper.Map<BoardDTO>(result.Content);
        return Ok(Result<BoardDTO>.SuccessResult(boardDTO));
    }

    [HttpPut]
    public async Task<IActionResult> Put(BoardDTO boardDTO)
    {
        Board board = _mapper.Map<Board>(boardDTO);
        var result = await _updateBoardUseCase.HandleAsync(board);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteBoardUseCase.HandleAsync(id);
        return Ok(result);
    }
}
