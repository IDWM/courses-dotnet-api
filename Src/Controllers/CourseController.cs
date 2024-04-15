using courses_dotnet_api.Src.DTOs.Course;
using courses_dotnet_api.Src.Extensions;
using courses_dotnet_api.Src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace courses_dotnet_api.Src.Controllers;

[Authorize]
public class CourseController(ICourseRepository courseRepository) : BaseApiController
{
    private readonly ICourseRepository _courseRepository = courseRepository;
    private readonly string[] _roles = ["admin", "teacher"];

    [HttpPost]
    public async Task<IResult> CreateCourse(CreateCourseDto createCourseDto) { }

    [HttpPost("add-student")]
    public async Task<IResult> AddStudentToCourse(
        [FromQuery] string courseName,
        [FromQuery] string studentRut
    ) { }

    [HttpGet("{name}")]
    public async Task<IResult> GetCourseByName(string name) { }
}
