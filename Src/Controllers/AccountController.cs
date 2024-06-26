using System.Security.Cryptography;
using System.Text;
using courses_dotnet_api.Src.DTOs.Account;
using courses_dotnet_api.Src.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace courses_dotnet_api.Src.Controllers;

public class AccountController(IAccountRepository accountRepository, IUserRepository userRepository)
    : BaseApiController
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAccountRepository _accountRepository = accountRepository;

    [HttpPost("register")]
    public async Task<IResult> Register(RegisterDto registerDto)
    {
        if (
            await _userRepository.UserExistsByEmailAsync(registerDto.Email)
            || await _userRepository.UserExistsByRutAsync(registerDto.Rut)
        )
        {
            return TypedResults.BadRequest("User already exists");
        }

        await _accountRepository.AddAccountAsync(registerDto);

        if (!await _accountRepository.SaveChangesAsync())
        {
            return TypedResults.BadRequest("Failed to save user");
        }

        AccountDto? accountDto = await _accountRepository.GetAccountAsync(registerDto.Email);

        return TypedResults.Ok(accountDto);
    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginDto loginDto)
    {
        CredentialDto? credentialDto = await _accountRepository.GetCredentialAsync(loginDto.Email);

        if (credentialDto is null)
        {
            return TypedResults.BadRequest("Credentials are invalid");
        }

        using var hmac = new HMACSHA512(credentialDto.PasswordSalt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        if (!computedHash.SequenceEqual(credentialDto.PasswordHash))
        {
            return TypedResults.BadRequest("Credentials are invalid");
        }

        AccountDto? accountDto = await _accountRepository.GetAccountAsync(loginDto.Email);

        return TypedResults.Ok(accountDto);
    }
}
