using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Repositories;

namespace Kanbardoo.Application.BoardUseCases;
public class GetBoardUseCase : IGetBoardUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBoardUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Board>> HandleAsync()
    {
        var boards = await _unitOfWork.BoardRepository.GetAsync();
        return boards;
    }

    public async Task<IEnumerable<Board>> HandleAsync(BoardFilters boardFilters)
    {
        var boards = await _unitOfWork.BoardRepository.GetAsync(boardFilters);
        return boards;
    }

    public async Task<Board> HandleAsync(int id)
    {
        var board = await _unitOfWork.BoardRepository.GetAsync(id);
        return board;
    }
}
