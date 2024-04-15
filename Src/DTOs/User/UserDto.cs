namespace courses_dotnet_api.Src.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public required string Rut { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
