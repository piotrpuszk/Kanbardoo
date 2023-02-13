using AutoMapper;
using Kanbardoo.Application.Contracts.BoardContracts;
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
        await _addBoardUseCase.HandleAsync(newBoard);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Get(BoardFiltersDTO boardFiltersDTO)
    {
        var boardFilters = _mapper.Map<BoardFilters>(boardFiltersDTO);
        IEnumerable<Board> boards = await _getBoardUseCase.HandleAsync(boardFilters);
        return Ok(boards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Board board = await _getBoardUseCase.HandleAsync(id);
        var boardDTO = _mapper.Map<BoardDTO>(board);
        return Ok(boardDTO);
    }

    [HttpPut]
    public async Task<IActionResult> Put(BoardDTO boardDTO)
    {
        Board board = _mapper.Map<Board>(boardDTO);
        await _updateBoardUseCase.HandleAsync(board);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _deleteBoardUseCase.HandleAsync(id);
        return Ok();
    }
}
