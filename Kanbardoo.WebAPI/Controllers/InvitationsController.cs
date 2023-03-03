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
    private readonly IAcceptInvitationUseCase _acceptInvitationUseCase;
    private readonly IGetInvitationsUseCase _getInvitationsUseCase;
    private readonly ICancelInvitationUseCase _cancelInvitationUseCase;
    private readonly IDeclineInvitationUseCase _declineInvitationUseCase;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public InvitationsController(IInviteUserUseCase inviteUserUseCase,
                                 IGetInvitationsUseCase getInvitationsUseCase,
                                 IMapper mapper,
                                 ILogger logger,
                                 ICancelInvitationUseCase cancelInvitationUseCase,
                                 IAcceptInvitationUseCase acceptInvitationUseCase,
                                 IDeclineInvitationUseCase declineInvitationUseCase)
    {
        _inviteUserUseCase = inviteUserUseCase;
        _mapper = mapper;
        _logger = logger;
        _getInvitationsUseCase = getInvitationsUseCase;
        _cancelInvitationUseCase = cancelInvitationUseCase;
        _acceptInvitationUseCase = acceptInvitationUseCase;
        _declineInvitationUseCase = declineInvitationUseCase;
    }

    [HttpPost("invite")]
    public async Task<IActionResult> Invite([FromBody] NewInvitationDTO invitationDTO)
    {
        var invitation = _mapper.Map<NewInvitation>(invitationDTO);
        var result = await _inviteUserUseCase.HandleAsync(invitation);
        return result.GetActionResult();
    }

    [HttpPost("accept")]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationDTO acceptInvitationDTO)
    {
        var acceptInvitation = _mapper.Map<AcceptInvitation>(acceptInvitationDTO);
        var result = await _acceptInvitationUseCase.HandleAsync(acceptInvitation);
        return result.GetActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> CancelInvitation([FromBody] CancelInvitationDTO invitationDTO)
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

    [HttpPost("decline")]
    public async Task<IActionResult> Decline([FromBody] DeclineInvitationDTO declineInvitationDTO)
    {
        var declineInvitation = _mapper.Map<DeclineInvitation>(declineInvitationDTO);
        var result = await _declineInvitationUseCase.HandleAsync(declineInvitation);
        return result.GetActionResult();
    }
}
