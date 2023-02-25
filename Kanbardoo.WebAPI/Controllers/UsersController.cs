using AutoMapper;
using Kanbardoo.Application.Contracts;
using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Domain.Models;
using Kanbardoo.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Kanbardoo.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ISignUpUseCase _signUpUseCase;
    private readonly ISignInUseCase _signInUseCase;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICreateToken _createToken;

    public UsersController(ISignUpUseCase signUpUseCase,
                           ILogger logger,
                           IMapper mapper,
                           ISignInUseCase signInUseCase,
                           ICreateToken createToken)
    {
        _signUpUseCase = signUpUseCase;
        _logger = logger;
        _mapper = mapper;
        _signInUseCase = signInUseCase;
        _createToken = createToken;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDTO)
    {
        var signUp = _mapper.Map<SignUp>(signUpDTO);
        var result = await _signUpUseCase.HandleAsync(signUp);
        return result.GetActionResult();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInDTO signInDTO)
    {
        var signIn = _mapper.Map<SignIn>(signInDTO);
        var result = await _signInUseCase.HandleAsync(signIn);

        if (!result.IsSuccess)
        {
            return result.GetActionResult();
        }

        var jwtToken = _createToken.Create(result.Content!);
        var userDTO = _mapper.Map<KanUserDTO>(result.Content!);
        var userWithTokenResult = Result<object>.SuccessResult(new { Token = jwtToken, LoggedUser = userDTO });
        return userWithTokenResult.GetActionResult();
    }
}
