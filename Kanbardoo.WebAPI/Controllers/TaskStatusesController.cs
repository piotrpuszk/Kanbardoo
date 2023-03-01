using AutoMapper;
using Kanbardoo.Application.Contracts.TaskStatusContracts;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TaskStatusesController : ControllerBase
{
    private readonly IGetTaskStatusesUseCase _getTaskStatusesUseCase;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public TaskStatusesController(IGetTaskStatusesUseCase getTaskStatusesUseCase,
                                  ILogger logger,
                                  IMapper mapper)
    {
        _getTaskStatusesUseCase = getTaskStatusesUseCase;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var useCaseResult = await _getTaskStatusesUseCase.HandleAsync();

        if (!useCaseResult.IsSuccess)
        {
            return useCaseResult.GetActionResult();
        }

        var taskStatuses = _mapper.Map<IEnumerable<KanTaskStatusDTO>>(useCaseResult.Content!);
        return Result<IEnumerable<KanTaskStatusDTO>>.SuccessResult(taskStatuses).GetActionResult();
    }
}
