using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Kanbardoo.Application.UserContracts;

public class SignInUseCase : ISignInUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public SignInUseCase(IUnitOfWork unitOfWork,
                         ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<User>> HandleAsync(SignIn signIn)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(signIn.Name);

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signIn.Password));

        if (Convert.ToBase64String(hash) != Convert.ToBase64String(user.PasswordHash))
        {
            return Result<User>.ErrorResult("Unauthenticated");
        }

        return Result<User>.SuccessResult(user);
    }
}
