using AutoMapper;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Authorize(Policy = PolicyName.Admin)]
[Route("api/[controller]")]
[ApiController]
public class UserClaimsController : ControllerBase
{
    private readonly IAddClaimToUserUseCase _addClaimToUser;
    private readonly IRevokeClaimFromUserUseCase _revokeClaimFromUser;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserClaimsController(IAddClaimToUserUseCase addClaimToUser,
                                IMapper mapper,
                                ILogger logger,
                                IRevokeClaimFromUserUseCase revokeClaimFromUser)
    {
        _addClaimToUser = addClaimToUser;
        _mapper = mapper;
        _logger = logger;
        _revokeClaimFromUser = revokeClaimFromUser;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] KanUserClaimDTO userClaimDTO)
    {
        var userClaim = _mapper.Map<KanUserClaimModel>(userClaimDTO);

        var result = await _addClaimToUser.HandleAsync(userClaim);

        return result.GetActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUserClaimDTO userClaimDTO)
    {
        var userClaim = _mapper.Map<KanUserClaimModel>(userClaimDTO);

        var result = await _revokeClaimFromUser.HandleAsync(userClaim);

        return result.GetActionResult();    
    }
}
