using courses_dotnet_api.Src.DTOs.Course;

namespace courses_dotnet_api.Src.Interfaces;

public interface ICourseRepository
{
    Task<bool> CourseExistsByNameAsync(string name);
    Task AddCourseAsync(CreateCourseDto createCourseDto);
    Task AddStudentToCourseAsync(string courseName, string studentRut);
    Task<CourseDto?> GetCourseByNameAsync(string name);
    Task<bool> SaveChangesAsync();
}
