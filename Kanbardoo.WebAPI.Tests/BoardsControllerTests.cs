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
        NewKanBoardDTO newBoardDTO = new NewKanBoardDTO()
        {
            Name = "Test",
        };

        NewKanBoard newBoard = new NewKanBoard()
        {
            Name = newBoardDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewKanBoard>(newBoardDTO)).Returns(newBoard);
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
        NewKanBoardDTO newBoardDTO = new NewKanBoardDTO()
        {
            Name = string.Empty,
        };

        NewKanBoard newBoard = new NewKanBoard()
        {
            Name = newBoardDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewKanBoard>(newBoardDTO)).Returns(newBoard);
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
        KanBoardFiltersDTO boardFiltersDTO = new()
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

        KanBoardFilters boardFilters = new()
        {
            BoardName = boardFiltersDTO.BoardName,
            OrderByClauses = new List<OrderByClause<KanBoard>>()
            {
                new()
                {
                    ColumnName = "Name",
                    Order = OrderByOrder.Desc,
                }
            },
        };

        var boards = new List<KanBoard>()
        {
            new KanBoard()
            {
                Name = "Test1",
                ID = 1,
            },
            new KanBoard()
            {
                Name = "Test2",
                ID = 2,
            }
        };

        var boardsDTO = new List<KanBoardDTO>()
        {
            new KanBoardDTO()
            {
                Name = "Test1",
                ID = 1,
            },
            new KanBoardDTO()
            {
                Name = "Test2",
                ID = 2,
            }
        };
        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<KanBoardFilters>())).ReturnsAsync(Result<IEnumerable<KanBoard>>.SuccessResult(boards));
        _mapper.Setup(e => e.Map<IEnumerable<KanBoardDTO>>(boards)).Returns(boardsDTO);

        var result = await _boardsController.Get(boardFiltersDTO) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var successResult = result.Value as SuccessResult<IEnumerable<KanBoardDTO>>;

        Assert.IsNotNull(successResult);
        Assert.IsNotNull(successResult.Content);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.That(successResult.Content, Is.EqualTo(boardsDTO));
    }

    [Test]
    public async Task Get_InvalidBoardFilters_ReturnsBadRequestWithErrorResult()
    {
        KanBoardFiltersDTO boardFiltersDTO = null!;

        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<KanBoardFilters>())).ReturnsAsync(Result<IEnumerable<KanBoard>>.ErrorResult(""));
        _mapper.Setup(e => e.Map<KanBoardFilters>(boardFiltersDTO)).Returns(() => null!);

        var result = await _boardsController.Get(boardFiltersDTO) as BadRequestObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var errorResult = result.Value as ErrorResult<IEnumerable<KanBoardDTO>>;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Put_ValidBoardDTO_ReturnsOkWithSuccessResult()
    {
        KanBoardDTO boardDTO = new()
        {
            ID = 1,
            Name= "Test",
            Status = new KanBoardStatusDTO()
            {
                ID = 1,
            },
            Owner = new KanUserDTO()
            {
                ID = 1,
            },
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        KanBoard board = new()
        {
            ID = 1,
            Name = "Test",
            Status = new KanBoardStatus()
            {
                ID = 1,
            },
            Owner = new KanUser()
            {
                ID = 1,
            },
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _mapper.Setup(e => e.Map<KanBoard>(boardDTO)).Returns(board);

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
        KanBoardDTO boardDTO = new()
        {
            ID = default,
        };

        KanBoard board = new()
        {
            ID = boardDTO.ID,
        };

        _mapper.Setup(e => e.Map<KanBoard>(boardDTO)).Returns(board);
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
        _addBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<NewKanBoard>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Post(It.IsAny<NewKanBoardDTO>()) as ObjectResult;

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
        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<KanBoardFilters>())).ReturnsAsync(Result<IEnumerable<KanBoard>>.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Get(It.IsAny<KanBoardFiltersDTO>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<IEnumerable<KanBoardDTO>>;

        Assert.NotNull(errorResult);
    }

    [Test]
    public async Task GetById_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _getBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result<KanBoard>.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Get(It.IsAny<int>()) as ObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<KanBoardDTO>;

        Assert.NotNull(errorResult);
    }

    [Test]
    public async Task Put_HandleAsyncReturnsInternalServerError_ReturnsInternalServerErrorWithErrorResult()
    {
        //Arrange
        _updateBoardUseCase.Setup(e => e.HandleAsync(It.IsAny<KanBoard>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        //Act
        var result = await _boardsController.Put(It.IsAny<KanBoardDTO>()) as ObjectResult;

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
