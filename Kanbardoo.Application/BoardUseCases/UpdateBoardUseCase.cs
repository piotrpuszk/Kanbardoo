using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public class UpdateBoardUseCase : IUpdateBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BoardToUpdateValidator _boardToUpdateValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;

    public UpdateBoardUseCase(IUnitOfWork unitOfWork,
                              ILogger logger,
                              BoardToUpdateValidator boardToUpdateValidator,
                              IBoardMembershipPolicy boardMembershipPolicy)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _boardToUpdateValidator = boardToUpdateValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
    }

    public async Task<Result> HandleAsync(KanBoard board)
    {
        var validationResult = await _boardToUpdateValidator.ValidateAsync(board);
        if (!validationResult.IsValid)
        {
            _logger.Error($"{nameof(UpdateBoardUseCase)}.{nameof(HandleAsync)} board is invalid");
            return Result.ErrorResult(ErrorMessage.GivenBoardInvalid);
        }

        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(board.ID);
        if (!authorizationResult.IsSuccess)
        {
            return authorizationResult;
        }

        try
        {
            await _unitOfWork.BoardRepository.UpdateAsync(board);
            await _unitOfWork.SaveChangesAsync();
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"{JsonConvert.SerializeObject(board)} \n\n {ex}");
            return Result.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
        
    }
}
