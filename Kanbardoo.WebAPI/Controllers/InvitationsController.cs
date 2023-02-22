using AutoMapper;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvitationsController : ControllerBase
{
    private readonly IInviteUserUseCase _inviteUserUseCase;
    private readonly IGetInvitationsUseCase _getInvitationsUseCase;
    private readonly ICancelInvitationUseCase _cancelInvitationUseCase;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public InvitationsController(IInviteUserUseCase inviteUserUseCase,
                                 IGetInvitationsUseCase getInvitationsUseCase,
                                 IMapper mapper,
                                 ILogger logger,
                                 ICancelInvitationUseCase cancelInvitationUseCase)
    {
        _inviteUserUseCase = inviteUserUseCase;
        _mapper = mapper;
        _logger = logger;
        _getInvitationsUseCase = getInvitationsUseCase;
        _cancelInvitationUseCase = cancelInvitationUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Invite([FromBody] NewInvitationDTO invitationDTO)
    {
        var invitation = _mapper.Map<NewInvitation>(invitationDTO);
        var result = await _inviteUserUseCase.HandleAsync(invitation);
        return result.GetActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> Invite([FromBody] CancelInvitationDTO invitationDTO)
    {
        var invitation = _mapper.Map<CancelInvitationModel>(invitationDTO);
        var result = await _cancelInvitationUseCase.HandleAsync(invitation);
        return result.GetActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Result<IEnumerable<Invitation>> usecaseResult = await _getInvitationsUseCase.HandleAsync();

        if (!usecaseResult.IsSuccess)
        {
            return usecaseResult.GetActionResult();
        }

        var invitationDTOs = _mapper.Map<IEnumerable<InvitationDTO>>(usecaseResult.Content);

        return Result<IEnumerable<InvitationDTO>>
                .SuccessResult(invitationDTOs)
                .GetActionResult();
    }
}
