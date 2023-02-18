using AutoMapper;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.Controllers;
using Kanbardoo.WebAPI.DTOs;
using Kanbardoo.WebAPI.FilterDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using System.Net;

namespace Kanbardoo.WebAPI.Tests;
internal class BoardsControllerTests
{
    private Mock<ILogger> _logger;
    private Mock<IAddBoardUseCase> _addBoardUseCase;
    private Mock<IUpdateBoardUseCase> _updateBoardUseCase;
    private Mock<IDeleteBoardUseCase> _deleteBoardUseCase;
    private Mock<IGetBoardUseCase> _getBoardUseCase;
    private Mock<IMapper> _mapper;
    private BoardsController _boardsController;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger>();
        _addBoardUseCase = new Mock<IAddBoardUseCase>();
        _updateBoardUseCase = new Mock<IUpdateBoardUseCase>();
        _deleteBoardUseCase = new Mock<IDeleteBoardUseCase>();
        _getBoardUseCase = new Mock<IGetBoardUseCase>();
        _mapper = new Mock<IMapper>();

        _boardsController = new BoardsController(
            _logger.Object,
            _addBoardUseCase.Object,
            _getBoardUseCase.Object,
            _updateBoardUseCase.Object,
            _mapper.Object,
            _deleteBoardUseCase.Object
            );
    }

    [Test]
    public async Task Post_ValidNewBoardDTO_ReturnsOkWithSuccessResult()
    {
        NewBoardDTO newBoardDTO = new NewBoardDTO()
        {
            Name = "Test",
        };

        NewBoard newBoard = new NewBoard()
        {
            Name = newBoardDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewBoard>(newBoardDTO)).Returns(newBoard);
        _addBoardUseCase.Setup(e => e.HandleAsync(newBoard)).ReturnsAsync(Result.SuccessResult());

        OkObjectResult result = (await _boardsController.Post(newBoardDTO) as OkObjectResult)!;

        Assert.IsNotNull(result);

        var successResult = result.Value as SuccessResult;

        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Post_EmptyNewBoardName_ReturnsBadRequestWithErrorResult()
    {
        NewBoardDTO newBoardDTO = new NewBoardDTO()
        {
            Name = string.Empty,
        };

        NewBoard newBoard = new NewBoard()
        {
            Name = newBoardDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewBoard>(newBoardDTO)).Returns(newBoard);
        _addBoardUseCase.Setup(e => e.HandleAsync(newBoard)).ReturnsAsync(Result.ErrorResult(""));

        var result = await _boardsController.Post(newBoardDTO) as BadRequestObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
    }

    [Test]
    public async Task Get_ValidBoardFilters_ReturnsSuccessResult()
    {
        BoardFiltersDTO boardFiltersDTO = new()
        {
            BoardName = string.Empty,
            OrderByClauses = new[]
            {
                new OrderByClauseDTO()
                {
                    ColumnName = "Name",
                    Order = OrderByOrder.Desc,
                }
            }
        };

        BoardFilters boardFilters = new()
        {
            BoardName = boardFiltersDTO.BoardName,
            OrderByClauses = new List<OrderByClause<Board>>()
            {
                new()
                {
                    ColumnName = "Name",
                    Order = OrderByOrder.Desc,
                }
            },
        };

        var boards = new List<Board>()
        {
            new Board()
            {
                Name = "Test1",
                ID = 1,
            },
            new Board()
            {
                Name = "Test2",
                ID = 2,
            }
        };

        var boardsDTO = new List<BoardDTO>()
        {
            new BoardDTO()
            {
                Name = "Test1",
                ID = 1,
            },
            new BoardDTO()
            {
                Name = "Test2",
                ID = 2,
            }
        };
        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<BoardFilters>())).ReturnsAsync(Result<IEnumerable<Board>>.SuccessResult(boards));
        _mapper.Setup(e => e.Map<IEnumerable<BoardDTO>>(boards)).Returns(boardsDTO);

        var result = await _boardsController.Get(boardFiltersDTO) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var successResult = result.Value as SuccessResult<IEnumerable<BoardDTO>>;

        Assert.IsNotNull(successResult);
        Assert.IsNotNull(successResult.Content);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.That(successResult.Content, Is.EqualTo(boardsDTO));
    }

    [Test]
    public async Task Get_InvalidBoardFilters_ReturnsBadRequestWithErrorResult()
    {
        BoardFiltersDTO boardFiltersDTO = null!;

        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<BoardFilters>())).ReturnsAsync(Result<IEnumerable<Board>>.ErrorResult(""));
        _mapper.Setup(e => e.Map<BoardFilters>(boardFiltersDTO)).Returns(() => null!);

        var result = await _boardsController.Get(boardFiltersDTO) as BadRequestObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var errorResult = result.Value as ErrorResult<IEnumerable<BoardDTO>>;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Put_ValidBoardDTO_ReturnsOkWithSuccessResult()
    {
        BoardDTO boardDTO = new()
        {
            ID = 1,
            Name= "Test",
            Status = new BoardStatusDTO()
            {
                ID = 1,
            },
            Owner = new UserDTO()
            {
                ID = 1,
            },
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        Board board = new()
        {
            ID = 1,
            Name = "Test",
            Status = new BoardStatus()
            {
                ID = 1,
            },
            Owner = new User()
            {
                ID = 1,
            },
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _mapper.Setup(e => e.Map<Board>(boardDTO)).Returns(board);

        _updateBoardUseCase.Setup(e => e.HandleAsync(board)).ReturnsAsync(Result.SuccessResult());

        var result = await _boardsController.Put(boardDTO) as OkObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var successResult = result.Value as SuccessResult;

        Assert.NotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Put_InvalidBoardDTO_ReturnsBadRequestWithErrorResult()
    {
        BoardDTO boardDTO = new()
        {
            ID = default,
        };

        Board board = new()
        {
            ID = boardDTO.ID,
        };

        _mapper.Setup(e => e.Map<Board>(boardDTO)).Returns(board);
        _updateBoardUseCase.Setup(e => e.HandleAsync(board)).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _boardsController.Put(boardDTO) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
        Assert.NotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Delete_ExistingId_ReturnsOkWithSuccessResult()
    {
        _deleteBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        var result = await _boardsController.Delete(It.IsAny<int>()) as OkObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var successResult = result.Value as SuccessResult;

        Assert.NotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Delete_NonExistingId_ReturnsBadRequestWithErrorResult()
    {
        _deleteBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _boardsController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
        Assert.NotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Post_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _addBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<NewBoard>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Post(It.IsAny<NewBoardDTO>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
    }

    [Test]
    public async Task Get_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<BoardFilters>())).ReturnsAsync(Result<IEnumerable<Board>>.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Get(It.IsAny<BoardFiltersDTO>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<IEnumerable<BoardDTO>>;

        Assert.NotNull(errorResult);
    }

    [Test]
    public async Task GetById_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result<Board>.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Get(It.IsAny<int>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<BoardDTO>;

        Assert.NotNull(errorResult);
    }

    [Test]
    public async Task Put_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _updateBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<Board>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Put(It.IsAny<BoardDTO>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
    }

    [Test]
    public async Task Delete_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _deleteBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Delete(It.IsAny<int>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
    }
}
