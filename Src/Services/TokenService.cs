using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using courses_dotnet_api.Src.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace courses_dotnet_api.Src.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly SymmetricSecurityKey _key =
        new(
            Encoding.UTF8.GetBytes(
                config["TokenKey"] ?? throw new InvalidOperationException("Token key not found")
            )
        );

    public string CreateToken(string rut, string nameRole)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, rut),
            new(ClaimTypes.Role, nameRole)
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
