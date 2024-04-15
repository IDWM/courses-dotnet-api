using System.ComponentModel.DataAnnotations;

namespace courses_dotnet_api.Src.DTOs.Course;

public class CreateCourseDto
{
    [StringLength(
        100,
        MinimumLength = 3,
        ErrorMessage = "Name must be between 3 and 100 characters"
    )]
    public required string Name { get; set; }
}
