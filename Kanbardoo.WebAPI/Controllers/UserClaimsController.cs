using AutoMapper;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserClaimsController : ControllerBase
{
    private readonly IAddClaimToUserUseCase _addClaimToUser;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserClaimsController(IAddClaimToUserUseCase addClaimToUser,
                                IMapper mapper,
                                ILogger logger)
    {
        _addClaimToUser = addClaimToUser;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] KanUserClaimDTO userClaimDTO)
    {
        var userClaim = _mapper.Map<KanUserClaim>(userClaimDTO);

        var result = await _addClaimToUser.HandleAsync(userClaim);

        return result.GetActionResult();
    }
}
