namespace courses_dotnet_api.Src.DTOs.Account;

public class CredentialDto
{
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
}
