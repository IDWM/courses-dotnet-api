namespace courses_dotnet_api.Src.Models;

public class User
{
    public int Id { get; set; }
    public required string Rut { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public int RoleId { get; set; }
    public required Role Role { get; set; }
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}
