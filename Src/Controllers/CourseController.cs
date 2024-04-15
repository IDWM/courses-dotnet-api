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
        if (!_roles.Contains(User.GetUserRole()))
        {
            return TypedResults.Unauthorized();
        }

        if (await _courseRepository.CourseExistsByNameAsync(createCourseDto.Name))
        {
            return TypedResults.BadRequest("Course already exists");
        }

        await _courseRepository.AddCourseAsync(createCourseDto);

        if (await _courseRepository.SaveChangesAsync())
        {
            return TypedResults.Created("Course created successfully");
        }

        return TypedResults.BadRequest("Failed to create course");
    }

    [HttpPost("add-student")]
    public async Task<IResult> AddStudentToCourse(
        [FromQuery] string courseName,
        [FromQuery] string studentRut
    )
    {
        if (!_roles.Contains(User.GetUserRole()))
        {
            return TypedResults.Unauthorized();
        }

        await _courseRepository.AddStudentToCourseAsync(courseName, studentRut);

        if (await _courseRepository.SaveChangesAsync())
        {
            return TypedResults.Created("Student added to course successfully");
        }

        return TypedResults.BadRequest("Failed to add student to course");
    }

    [HttpGet("{name}")]
    public async Task<IResult> GetCourseByName(string name)
    {
        if (!_roles.Contains(User.GetUserRole()))
        {
            return TypedResults.Unauthorized();
        }

        CourseDto? course = await _courseRepository.GetCourseByNameAsync(name);

        if (course is null)
        {
            return TypedResults.NotFound("Course not found");
        }

        return TypedResults.Ok(course);
    }
}
