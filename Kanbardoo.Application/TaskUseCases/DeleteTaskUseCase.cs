using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.Application.TaskUseCases;

public class DeleteTaskUseCase : IDeleteTaskUseCase
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly KanTaskIdToDeleteValidator _kanTaskIdToDeleteValidator;

    public DeleteTaskUseCase(ILogger logger,
                           IUnitOfWork unitOfWork,
                           KanTaskIdToDeleteValidator kanTaskIdToDeleteValidator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _kanTaskIdToDeleteValidator = kanTaskIdToDeleteValidator;
    }

    public async Task<Result> HandleAsync(int id)
    {
        var validationResult = await _kanTaskIdToDeleteValidator.ValidateAsync(id);
        if (!validationResult.IsValid)
        {
            _logger.Error($"A task with the given id ({id}) does not exist");
            return Result.ErrorResult("A task with the given id does not exist");
        }

        try
        {
            await _unitOfWork.TaskRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error($"Internal server error: {id} \n\n {ex}");
            return Result.ErrorResult("Internal server error", HttpStatusCode.InternalServerError);
        }
        
    }
}
