using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Kanbardoo.Domain.Repositories;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Kanbardoo.Application.UserContracts;
public class SignUpUseCase : ISignUpUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public SignUpUseCase(IUnitOfWork unitOfWork,
                         ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(SignUp signUp)
    {
        using var hmac = new HMACSHA512();

        var user = new User()
        {
            Name = signUp.Name,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signUp.Password)),
            PasswordSalt = hmac.Key,
        };

        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.SuccessResult();
    }
}
