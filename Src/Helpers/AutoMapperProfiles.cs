using AutoMapper;
using courses_dotnet_api.Src.DTOs.Account;
using courses_dotnet_api.Src.DTOs.Course;
using courses_dotnet_api.Src.DTOs.User;
using courses_dotnet_api.Src.Models;

namespace courses_dotnet_api.Src.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, CredentialDto>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<Course, CourseDto>();
        CreateMap<User, UserDto>();
    }
}
