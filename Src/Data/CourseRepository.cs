using AutoMapper;
using AutoMapper.QueryableExtensions;
using courses_dotnet_api.Src.DTOs.Course;
using courses_dotnet_api.Src.Interfaces;
using courses_dotnet_api.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace courses_dotnet_api.Src.Data;

public class CourseRepository(DataContext dataContext, IMapper mapper) : ICourseRepository
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;

    public async Task AddCourseAsync(CreateCourseDto createCourseDto)
    {
        createCourseDto.Name = createCourseDto.Name.ToLower();
        Course course = _mapper.Map<Course>(createCourseDto);
        await _dataContext.Courses.AddAsync(course);
    }

    public async Task AddStudentToCourseAsync(string courseName, string studentRut)
    {
        courseName = courseName.ToLower();
        studentRut = studentRut.ToLower();

        Course? course = await _dataContext
            .Courses.Include(course => course.Students)
            .FirstOrDefaultAsync(x => x.Name == courseName);

        if (course is null)
            return;

        User? student = await _dataContext.Users.FirstOrDefaultAsync(x => x.Rut == studentRut);

        if (student is null)
            return;

        student.CourseId = course.Id;
        student.Course = course;
    }

    public async Task<bool> CourseExistsByNameAsync(string name)
    {
        name = name.ToLower();
        return await _dataContext.Courses.AnyAsync(x => x.Name == name);
    }

    public async Task<CourseDto?> GetCourseByNameAsync(string name)
    {
        name = name.ToLower();
        return await _dataContext
            .Courses.Where(x => x.Name == name)
            .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return 0 < await _dataContext.SaveChangesAsync();
    }
}
