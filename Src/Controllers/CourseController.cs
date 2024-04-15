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
    public async Task<IResult> CreateCourse(CreateCourseDto createCourseDto)
    {
        if (await _courseRepository.CourseExistsByNameAsync(createCourseDto.Name))
        {
            return TypedResults.BadRequest("Course already exists");
        }

        await _courseRepository.AddCourseAsync(createCourseDto);
        await _courseRepository.SaveChangesAsync();
        return TypedResults.Created("Course created successfully");
    }

    [HttpPost("add-student")]
    public async Task<IResult> AddStudentToCourse(
        [FromQuery] string courseName,
        [FromQuery] string studentRut
    )
    {
        await _courseRepository.AddStudentToCourseAsync(courseName, studentRut);

        if (await _courseRepository.SaveChangesAsync())
        {
            return TypedResults.Created("Student added to course");
        }

        return TypedResults.BadRequest("Error adding student to course");
    }

    [HttpGet("{name}")]
    public async Task<IResult> GetCourseByName(string name)
    {
        var course = await _courseRepository.GetCourseByNameAsync(name);
        if (course == null)
        {
            return TypedResults.NotFound("Course not found");
        }

        return TypedResults.Ok(course);
    }
}
