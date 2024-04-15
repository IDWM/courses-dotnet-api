namespace courses_dotnet_api.Src.Models;

public class Course
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<User> Students { get; set; } = [];
}
