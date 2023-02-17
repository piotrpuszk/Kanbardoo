using FluentValidation;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.BoardUseCases;
public class GetBoardUseCase : IGetBoardUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BoardFiltersValidator _boardFiltersValidator;

    public GetBoardUseCase(IUnitOfWork unitOfWork,
                           ILogger logger,
                           BoardFiltersValidator boardFiltersValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _boardFiltersValidator = boardFiltersValidator;
    }

    public async Task<Result<IEnumerable<Board>>> HandleAsync()
    {
        var boards = await _unitOfWork.BoardRepository.GetAsync();
        return Result<IEnumerable<Board>>.SuccessResult(boards);
    }

    public async Task<Result<IEnumerable<Board>>> HandleAsync(BoardFilters boardFilters)
    {
        var validationResult = _boardFiltersValidator.Validate(boardFilters);
        if (!validationResult.IsValid)
        {
            _logger.Error($"Board filters are invalid: {JsonConvert.SerializeObject(boardFilters)}");
            return Result<IEnumerable<Board>>.ErrorResult($"Board filters are invalid");
        }

        try
        {
            var boards = await _unitOfWork.BoardRepository.GetAsync(boardFilters!);
            return Result<IEnumerable<Board>>.SuccessResult(boards);
        }
        catch (Exception ex)
        {
            _logger.Error($"{JsonConvert.SerializeObject(boardFilters)} \n\n {ex}");
            return Result<IEnumerable<Board>>.ErrorResult($"Internal server error");
        }
    }

    public async Task<Result<Board>> HandleAsync(int id)
    {
        Board board;
        try
        {
            board = await _unitOfWork.BoardRepository.GetAsync(id);
        }
        catch(Exception ex)
        {
            _logger.Error($"Internal server error GetBoardUseCase.HandleAsync({id}) \n\n {ex}");
            return Result<Board>.ErrorResult($"Internal server error");
        }

        if (!board.Exists())
        {
            _logger.Error("A board with ID {id} does not exist");
            return Result<Board>.ErrorResult($"A board with ID {id} does not exist");
        }

        return Result<Board>.SuccessResult(board);
    }
}
