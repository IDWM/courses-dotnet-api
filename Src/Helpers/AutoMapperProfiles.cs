using AutoMapper;
using courses_dotnet_api.Src.DTOs.Account;
using courses_dotnet_api.Src.Models;

namespace courses_dotnet_api.Src.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, CredentialDto>();
    }
}
