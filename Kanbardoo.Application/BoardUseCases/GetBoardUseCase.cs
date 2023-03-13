using FluentValidation;
using Kanbardoo.Application.Authorization;
using Kanbardoo.Application.Authorization.Policies;
using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public class GetBoardUseCase : IGetBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BoardFiltersValidator _boardFiltersValidator;
    private readonly IBoardMembershipPolicy _boardMembershipPolicy;

    public GetBoardUseCase(IUnitOfWork unitOfWork,
                           ILogger logger,
                           BoardFiltersValidator boardFiltersValidator,
                           IBoardMembershipPolicy boardMembershipPolicy)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _boardFiltersValidator = boardFiltersValidator;
        _boardMembershipPolicy = boardMembershipPolicy;
    }

    public async Task<Result<IEnumerable<KanBoard>>> HandleAsync()
    {
        var boards = await _unitOfWork.BoardRepository.GetAsync();
        return Result<IEnumerable<KanBoard>>.SuccessResult(boards);
    }

    public async Task<Result<IEnumerable<KanBoard>>> HandleAsync(KanBoardFilters boardFilters)
    {
        var validationResult = _boardFiltersValidator.Validate(boardFilters);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Board filters are invalid: {JsonConvert.SerializeObject(boardFilters)}");
            return Result<IEnumerable<KanBoard>>.ErrorResult(ErrorMessage.BoardFiltersInvalid);
        }

        try
        {
            var boards = await _unitOfWork.BoardRepository.GetAsync(boardFilters!);
            return Result<IEnumerable<KanBoard>>.SuccessResult(boards);
        }
        catch (Exception ex)
        {
            _logger.Error($"{JsonConvert.SerializeObject(boardFilters)} \n\n {ex}");
            return Result<IEnumerable<KanBoard>>.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<KanBoard>> HandleAsync(int id)
    {
        KanBoard board;
        try
        {
            board = await _unitOfWork.BoardRepository.GetAsync(id);
        }
        catch(Exception ex)
        {
            _logger.Error($"Internal server error GetBoardUseCase.HandleAsync({id}) \n\n {ex}");
            return Result<KanBoard>.ErrorResult(ErrorMessage.InternalServerError, HttpStatusCode.InternalServerError);
        }

        if (!board.Exists())
        {
            _logger.Error("A board with ID {id} does not exist");
            return Result<KanBoard>.ErrorResult($"A board with ID {id} does not exist");
        }

        var authorizationResult = await _boardMembershipPolicy.AuthorizeAsync(id);
        if (!authorizationResult.IsSuccess)
        {
            return Result<KanBoard>.ErrorResult(authorizationResult.Errors!, HttpStatusCode.Forbidden);
        }

        return Result<KanBoard>.SuccessResult(board);
    }
}
