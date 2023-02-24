using AutoMapper;
using Kanbardoo.Application.Contracts.UserRolesContracts;
using Kanbardoo.Domain.Authorization;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kanbardoo.WebAPI.Controllers;
[Authorize(Policy = PolicyName.Admin)]
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IGrantRoleToUserUseCase _grantRoleToUserUseCase;
    private readonly IRevokeRoleFromUserUseCase _revokeRoleFromUserUseCase;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UserRolesController(IGrantRoleToUserUseCase grantRoleToUserUseCase,
                               IRevokeRoleFromUserUseCase revokeRoleFromUserUseCase,
                               IMapper mapper,
                               ILogger logger)
    {
        _grantRoleToUserUseCase = grantRoleToUserUseCase;
        _revokeRoleFromUserUseCase = revokeRoleFromUserUseCase;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("boards")]
    public async Task<IActionResult> Post([FromBody] UserBoardRoleGrantDTO userBoardRoleGrantDTO)
    {
        var grant = _mapper.Map<UserBoardRoleGrantModel>(userBoardRoleGrantDTO);

        var result = await _grantRoleToUserUseCase.HandleAsync(grant);

        return result.GetActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(UserRoleRevokeDTO userRoleRevokeDTO)
    {
        var userRoleRevoke = _mapper.Map<UserRoleRevokeModel>(userRoleRevokeDTO);

        var result = await _revokeRoleFromUserUseCase.HandleAsync(userRoleRevoke);

        return result.GetActionResult();
    }
}
