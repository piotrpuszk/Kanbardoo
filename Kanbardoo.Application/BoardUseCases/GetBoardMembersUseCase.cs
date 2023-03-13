using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;

public class GetBoardMembersUseCase : IGetBoardMembersUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;

    public GetBoardMembersUseCase(ILogger logger,
                                  IUnitOfWork unitOfWork,
                                  IBoardMembershipPolicy boardMembershipPolicy)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _boardMembershipPolicy = boardMembershipPolicy;
    }

    public async Task<Result<IEnumerable<KanBoardUser>>> HandleAsync(int boardID)
    {
        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(boardID);
        if (!authorizationResult.IsSuccess)
        {
            return Result<IEnumerable<KanBoardUser>>.ErrorResult(authorizationResult.Errors!, HttpStatusCode.Forbidden);
        }

        var users = await _unitOfWork.BoardRepository.GetBoardMembers(boardID);

        var boardUsers = users.Select(e => new KanBoardUser 
        { 
            ID = e.ID, 
            UserName = e.UserName, 
            CreationDate = e.CreationDate, 
            RoleName = nameof(KanRoleID.Member) 
        });

        return Result<IEnumerable<KanBoardUser>>.SuccessResult(boardUsers);
    }
}