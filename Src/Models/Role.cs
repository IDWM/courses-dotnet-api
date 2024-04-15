namespace courses_dotnet_api.Src.Models;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<User> Users { get; set; } = [];
}
