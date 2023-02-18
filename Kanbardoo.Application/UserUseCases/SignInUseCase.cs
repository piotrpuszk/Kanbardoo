using Kanbardoo.Application.Constants;
using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Kanbardoo.Application.UserContracts;

public class SignInUseCase : ISignInUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
    private readonly SignInValidator _signInValidator;

    public SignInUseCase(IUnitOfWork unitOfWork,
                         ILogger logger,
                         SignInValidator signInValidator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _signInValidator = signInValidator;
    }

    public async Task<Result<User>> HandleAsync(SignIn signIn)
    {
        var validationResult = _signInValidator.Validate(signIn);
        if (!validationResult.IsValid)
        {
            if (signIn is not null) _logger.Error($"The sign in data are invalid {JsonConvert.SerializeObject(signIn)}");
            else _logger.Error("The sign in data are null");
            return Result<User>.ErrorResult(ErrorMessage.SignInDataInvalid);
        }

        var user = await _unitOfWork.UserRepository.GetAsync(signIn.Name);

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signIn.Password));

        if (Convert.ToBase64String(hash) != Convert.ToBase64String(user.PasswordHash))
        {
            return Result<User>.ErrorResult(ErrorMessage.Unauthenticated, HttpStatusCode.Unauthorized);
        }

        return Result<User>.SuccessResult(user);
    }
}
