﻿using AutoMapper;
using Kanbardoo.Application.Contracts.TableContracts;
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
internal class TablesControllerTests
{
    private Mock<ILogger> _logger;
    private Mock<IAddTableUseCase> _addTableUseCase;
    private Mock<IUpdateTableUseCase> _updateTableUseCase;
    private Mock<IDeleteTableUseCase> _deleteTableUseCase;
    private Mock<IGetTableUseCase> _getTableUseCase;
    private Mock<IMapper> _mapper;
    private TablesController _tablesController;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger>();
        _addTableUseCase = new Mock<IAddTableUseCase>();
        _updateTableUseCase = new Mock<IUpdateTableUseCase>();
        _deleteTableUseCase = new Mock<IDeleteTableUseCase>();
        _getTableUseCase = new Mock<IGetTableUseCase>();
        _mapper = new Mock<IMapper>();

        _tablesController = new TablesController(
            _logger.Object,
            _mapper.Object,
            _addTableUseCase.Object,
            _updateTableUseCase.Object,
            _deleteTableUseCase.Object,
            _getTableUseCase.Object
            );
    }

    [Test]
    public async Task Post_ValidNewTableDTO_ReturnsOkWithSuccessResult()
    {
        NewKanTableDTO newTableDTO = new NewKanTableDTO()
        {
            Name = "Test",
        };

        NewKanTable newTable = new NewKanTable()
        {
            Name = newTableDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewKanTable>(newTableDTO)).Returns(newTable);
        _addTableUseCase.Setup(e => e.HandleAsync(newTable)).ReturnsAsync(Result.SuccessResult());

        var result = await _tablesController.Post(newTableDTO) as OkObjectResult;

        Assert.IsNotNull(result);

        var successResult = result.Value as SuccessResult;

        Assert.IsNotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Post_EmptyNewTableName_ReturnsBadRequestWithErrorResult()
    {
        NewKanTableDTO newTableDTO = new NewKanTableDTO()
        {
            Name = string.Empty,
        };

        NewKanTable newTable = new NewKanTable()
        {
            Name = newTableDTO.Name,
        };

        _mapper.Setup(e => e.Map<NewKanTable>(newTableDTO)).Returns(newTable);
        _addTableUseCase.Setup(e => e.HandleAsync(newTable)).ReturnsAsync(Result.ErrorResult(""));

        var result = await _tablesController.Post(newTableDTO) as BadRequestObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsFalse(errorResult.IsSuccess);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
    }

    [Test]
    public async Task Get_AllTables_ReturnsSuccessResult()
    {
        var tables = new List<KanTable>()
        {
            new KanTable()
            {
                Name = "Test1",
                ID = 1,
            },
            new KanTable()
            {
                Name = "Test2",
                ID = 2,
            }
        };

        var tablesDTO = new List<KanTableDTO>()
        {
            new KanTableDTO()
            {
                Name = "Test1",
                ID = 1,
            },
            new KanTableDTO()
            {
                Name = "Test2",
                ID = 2,
            }
        };
        _getTableUseCase.Setup(e => e.HandleAsync()).ReturnsAsync(Result<IEnumerable<KanTable>>.SuccessResult(tables));
        _mapper.Setup(e => e.Map<IEnumerable<KanTableDTO>>(tables)).Returns(tablesDTO);

        var result = await _tablesController.Get() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);

        var successResult = result.Value as SuccessResult<IEnumerable<KanTableDTO>>;

        Assert.IsNotNull(successResult);
        Assert.IsNotNull(successResult.Content);
        Assert.IsTrue(successResult.IsSuccess);
        Assert.That(successResult.Content, Is.EqualTo(tablesDTO));
    }

    [Test]
    public async Task Put_ValidTableDTO_ReturnsOkWithSuccessResult()
    {
        KanTableDTO tableDTO = new()
        {
            ID = 1,
            Name= "Test",
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        KanTable table = new()
        {
            ID = 1,
            Name = "Test",
            CreationDate = new DateTime(2000, 1, 1, 12, 0, 0),
        };

        _mapper.Setup(e => e.Map<KanTable>(tableDTO)).Returns(table);

        _updateTableUseCase.Setup(e => e.HandleAsync(table)).ReturnsAsync(Result.SuccessResult());

        var result = await _tablesController.Put(tableDTO) as OkObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var successResult = result.Value as SuccessResult;

        Assert.NotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Put_InvalidTableDTO_ReturnsBadRequestWithErrorResult()
    {
        KanTableDTO tableDTO = new()
        {
            ID = default,
        };

        KanTable table = new()
        {
            ID = tableDTO.ID,
        };

        _mapper.Setup(e => e.Map<KanTable>(tableDTO)).Returns(table);
        _updateTableUseCase.Setup(e => e.HandleAsync(table)).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _tablesController.Put(tableDTO) as BadRequestObjectResult;

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
        _deleteTableUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.SuccessResult());

        var result = await _tablesController.Delete(It.IsAny<int>()) as OkObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var successResult = result.Value as SuccessResult;

        Assert.NotNull(successResult);
        Assert.IsTrue(successResult.IsSuccess);
    }

    [Test]
    public async Task Delete_NonExistingId_ReturnsBadRequestWithErrorResult()
    {
        _deleteTableUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.ErrorResult("error"));

        var result = await _tablesController.Delete(It.IsAny<int>()) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Value);

        var errorResult = result.Value as ErrorResult;

        Assert.NotNull(errorResult);
        Assert.NotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Get_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _getTableUseCase.Setup(e => e.HandleAsync()).ReturnsAsync(Result<IEnumerable<KanTable>>.ErrorResult("Internal server error", HttpStatusCode.InternalServerError));

        var result = await _tablesController.Get() as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<IEnumerable<KanTableDTO>>;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task GetById_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _getTableUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result<KanTable>.ErrorResult("Internal server error", HttpStatusCode.InternalServerError));

        var result = await _tablesController.Get(It.IsAny<int>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult<KanTableDTO>;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Post_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _addTableUseCase.Setup(e => e.HandleAsync(It.IsAny<NewKanTable>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        var result = await _tablesController.Post(It.IsAny<NewKanTableDTO>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Put_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _updateTableUseCase.Setup(e => e.HandleAsync(It.IsAny<KanTable>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        var result = await _tablesController.Put(It.IsAny<KanTableDTO>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }

    [Test]
    public async Task Delete_GettingFromDBError_ReturnsInternalServerErrorWithErrorResult()
    {
        _deleteTableUseCase.Setup(e => e.HandleAsync(It.IsAny<int>())).ReturnsAsync(Result.ErrorResult("", HttpStatusCode.InternalServerError));

        var result = await _tablesController.Delete(It.IsAny<int>()) as ObjectResult;

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
        Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

        var errorResult = result.Value as ErrorResult;

        Assert.IsNotNull(errorResult);
        Assert.IsNotNull(errorResult.Errors);
        Assert.IsNotEmpty(errorResult.Errors);
        Assert.IsFalse(errorResult.IsSuccess);
    }
}
