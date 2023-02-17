using Kanbardoo.Application.Results;
using Kanbardoo.Domain.Models;

namespace Kanbardoo.Application.Contracts.UserContracts;
public interface ISignUpUseCase
{
    Task<Result> HandleAsync(SignUp signUp); 
}
