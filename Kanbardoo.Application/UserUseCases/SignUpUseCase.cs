using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Kanbardoo.Application.UserContracts;
public class SignUpUseCase : ISignUpUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly SignUpValidator _signUpValidator;

    public SignUpUseCase(IUnitOfWork unitOfWork,
                         ILogger logger,
                         SignUpValidator signUpValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _signUpValidator = signUpValidator;
    }

    public async Task<Result> HandleAsync(SignUp signUp)
    {
        var validationResult = _signUpValidator.Validate(signUp);
        if (!validationResult.IsValid)
        {
            if (signUp is not null) _logger.Error($"The sign up data are invalid {JsonConvert.SerializeObject(signUp)}");
            else _logger.Error("The sign up data are null");
            return Result.ErrorResult(ErrorMessage.SignUpDataInvalid);
        }

        using var hmac = new HMACSHA512();

        var user = new KanUser()
        {
            UserName = signUp.Name,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signUp.Password)),
            PasswordSalt = hmac.Key,
            CreationDate= DateTime.UtcNow,
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.SuccessResult();
    }
}
