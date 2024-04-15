using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using courses_dotnet_api.Src.DTOs.Account;
using courses_dotnet_api.Src.Interfaces;
using courses_dotnet_api.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace courses_dotnet_api.Src.Data;

public class AccountRepository(DataContext dataContext, IMapper mapper, ITokenService tokenService)
    : IAccountRepository
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<bool> AccountExistsByEmailAsync(string email)
    {
        email = email.ToLower();
        return await _dataContext.Users.AnyAsync(x => x.Email == email);
    }

    public async Task AddAccountAsync(RegisterDto registerDto)
    {
        Role studentRole = await _dataContext.Roles.FirstAsync(x => x.Name == "Student");
        using var hmac = new HMACSHA512();

        User user =
            new()
            {
                Rut = registerDto.Rut.ToLower(),
                Name = registerDto.Name.ToLower(),
                Email = registerDto.Email.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Role = studentRole
            };

        await _dataContext.Users.AddAsync(user);
    }

    public async Task<AccountDto?> GetAccountAsync(string email)
    {
        email = email.ToLower();
        User? user = await _dataContext
            .Users.Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return null;
        }

        AccountDto accountDto =
            new()
            {
                Rut = user.Rut,
                Name = user.Name,
                Email = user.Email,
                Token = _tokenService.CreateToken(user.Rut, user.Role.Name)
            };

        return accountDto;
    }

    public async Task<CredentialDto?> GetCredentialAsync(string email)
    {
        email = email.ToLower();
        User? user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        return _mapper.Map<CredentialDto>(user);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return 0 < await _dataContext.SaveChangesAsync();
    }
}
