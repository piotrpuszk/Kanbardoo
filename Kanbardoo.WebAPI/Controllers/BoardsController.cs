using AutoMapper;
using Kanbardoo.Application.Authorization;
using Kanbardoo.Application.Authorization.Requirements;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Kanbardoo.WebAPI.FilterDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public sealed class BoardsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IAddBoardUseCase _addBoardUseCase;
    private readonly IGetBoardUseCase _getBoardUseCase;
    private readonly IUpdateBoardUseCase _updateBoardUseCase;
    private readonly IDeleteBoardUseCase _deleteBoardUseCase;
    private readonly IGetBoardMembersUseCase _getBoardMembersUseCase;
    private readonly IMapper _mapper;

    public BoardsController(ILogger logger,
                           IAddBoardUseCase addBoardUseCase,
                           IGetBoardUseCase getBoardUseCase,
                           IUpdateBoardUseCase updateBoardUseCase,
                           IMapper mapper,
                           IDeleteBoardUseCase deleteBoardUseCase,
                           IGetBoardMembersUseCase getBoardMembersUseCase)
    {
        _logger = logger;
        _addBoardUseCase = addBoardUseCase;
        _getBoardUseCase = getBoardUseCase;
        _updateBoardUseCase = updateBoardUseCase;
        _mapper = mapper;
        _deleteBoardUseCase = deleteBoardUseCase;
        _getBoardMembersUseCase = getBoardMembersUseCase;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Post(NewKanBoardDTO newBoardDTO)
    {
        NewKanBoard newBoard = _mapper.Map<NewKanBoard>(newBoardDTO);
        var result = await _addBoardUseCase.HandleAsync(newBoard);
        return result.GetActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Get(KanBoardFiltersDTO boardFiltersDTO)
    {
        var boardFilters = _mapper.Map<KanBoardFilters>(boardFiltersDTO);
        boardFilters.OwnerID = int.Parse(HttpContext.User.FindFirstValue(KanClaimName.ID)!);
        var result = await _getBoardUseCase.HandleAsync(boardFilters);

        if (!result.IsSuccess)
        {
            return Result<IEnumerable<KanBoardDTO>>.ErrorResult(result.Errors!, result.HttpCode).GetActionResult();
        }

        var boardDTOs = _mapper.Map<IEnumerable<KanBoardDTO>>(result.Content);
        return Result<IEnumerable<KanBoardDTO>>.SuccessResult(boardDTOs).GetActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _getBoardUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return Result<KanBoardDTO>.ErrorResult(result.Errors!, result.HttpCode).GetActionResult();
        }

        var boardDTO = _mapper.Map<KanBoardDTO>(result.Content);
        return Result<KanBoardDTO>.SuccessResult(boardDTO).GetActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> Put(KanBoardDTO boardDTO)
    {
        KanBoard board = _mapper.Map<KanBoard>(boardDTO);
        var result = await _updateBoardUseCase.HandleAsync(board);
        return result.GetActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteBoardUseCase.HandleAsync(id);
        return result.GetActionResult();
    }

    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetMembers(int id)
    {
        var result = await _getBoardMembersUseCase.HandleAsync(id);

        if (!result.IsSuccess)
        {
            return result.GetActionResult();
        }

        var userDTOs = _mapper.Map<IEnumerable<KanBoardUserDTO>>(result.Content);

        return Result<IEnumerable<KanBoardUserDTO>>.SuccessResult(userDTOs).GetActionResult();
    }
}
