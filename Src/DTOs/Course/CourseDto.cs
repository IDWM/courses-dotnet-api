using courses_dotnet_api.Src.DTOs.User;

namespace courses_dotnet_api.Src.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<UserDto> Students { get; set; }
}
