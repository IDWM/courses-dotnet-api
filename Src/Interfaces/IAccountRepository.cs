using courses_dotnet_api.Src.DTOs.Account;

namespace courses_dotnet_api.Src.Interfaces;

public interface IAccountRepository
{
    Task<bool> AccountExistsByEmailAsync(string email);
    Task AddAccountAsync(RegisterDto registerDto);
    Task<AccountDto?> GetAccountAsync(string email);
    Task<CredentialDto?> GetCredentialAsync(string email);
    Task<bool> SaveChangesAsync();
}
